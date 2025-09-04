using UnityEngine;
using TMPro;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine.Events;

public class LevelController : MonoBehaviour
{
    [Serializable]
    public class LevelData {
        public int Columns;
        public int Rows;
        public int Difficulty;
        public int Movements;
    }


    [SerializeField]
    private CardController _cardPrefab;

    [Header ("UI Settings")]
    [SerializeField]
    private TMPro.TMP_Text _levelText;
    [SerializeField]
    private TMPro.TMP_Text _movesText;
    [SerializeField]
    private GameObject _resetButton;

    [Header ("Level Settings")]
    [SerializeField]
    private List<LevelData> _levels = new List<LevelData>();

    private List<CardController> _cards = new List<CardController>();
    private int _movementsUser = 0;
    private CardController _activeCard = null; /* Carta que se está jugando actualmente */
    private bool _blockInput = true;
    private int _level = 0;

    public void StartLevel () {

        _resetButton.SetActive(false);
        if (_levels[_level].Difficulty > _cardPrefab.MaxCardTypes) {
            Debug.Assert(false);
            _levels[_level].Difficulty = Math.Min(_levels[_level].Difficulty, _cardPrefab.MaxCardTypes);
        }

        Debug.Assert((_levels[_level].Rows * _levels[_level].Columns) % 2 == 0);
        _cards.ForEach(card => Destroy(card.gameObject));
        _cards.Clear();

        /* Todas las cartas que se pueden crear */
        List<int> cardIndicesTypes = new List<int>();
        for (int i = 0; i < _cardPrefab.MaxCardTypes; i++) {
            cardIndicesTypes.Add(i);
        }

        /* Seleccion de las cartas que van a jugarse */
        List<int> gameTypes = new List<int>();
        for (int i = 0; i < _levels[_level].Difficulty; i++) {
            int chosenType = cardIndicesTypes[UnityEngine.Random.Range(0, cardIndicesTypes.Count)];
            cardIndicesTypes.Remove(chosenType);
            gameTypes.Add(chosenType);
        }

        /* Duplicar las cartas */
        List<int> chosenTypes = new List<int>();
        for (int i = 0; i < (_levels[_level].Rows * _levels[_level].Columns) / 2; i++) {
            int chosenType = gameTypes[UnityEngine.Random.Range(0, gameTypes.Count)];
            chosenTypes.Add(chosenType);
            chosenTypes.Add(chosenType);
        }


        for (int i = 0; i < _levels[_level].Rows; i++) {
            for (int j = 0; j < _levels[_level].Columns; j++) {
                Vector3 offset = new Vector3((_levels[_level].Columns - 2f) * _cardPrefab.CardSize, (_levels[_level].Rows - 3.2f) * _cardPrefab.CardSize, 0) * 0.5f;
                Vector3 position = new Vector3(j * _cardPrefab.CardSize, i * _cardPrefab.CardSize, 0);
                var card = Instantiate(_cardPrefab, position - offset, Quaternion.identity);
                /* Selección de carta */
                card.CardType = chosenTypes[UnityEngine.Random.Range(0, chosenTypes.Count)];
                chosenTypes.Remove(card.CardType);

                card.onClicked.AddListener(OnCardClicked);
                _cards.Add(card);   
            }
        }

        _blockInput = false;
        _movementsUser = 0;
        _levelText.text = $"Level: {_level}";
        _movesText.text = $"Moves: {_levels[_level].Movements }";
    }

    private void OnCardClicked(CardController card)
    {
        if (_blockInput) return;
        
        _blockInput = true;
        if (_activeCard == null)
        {
            StartCoroutine(SelectCard(card));
            return;
        }

        _movementsUser++;
        _movesText.text = $"Moves: {_levels[_level].Movements - _movementsUser}";

        if (_activeCard.CardType == card.CardType)
        {
            
            StartCoroutine(Score(card));
            return;
        }

        StartCoroutine(Fail(card));

        
    }

    private IEnumerator SelectCard(CardController card)
    {
        _activeCard = card;
        _activeCard.Reveal();
        yield return new WaitForSeconds(0.5f);
        _blockInput = false;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _level = PlayerPrefs.GetInt("Level", 0);
        StartLevel();
    }

    private IEnumerator Score (CardController card){
        card.Reveal();
        yield return new WaitForSeconds(1f);
        _cards.Remove(card);
        _cards.Remove(_activeCard);
        Destroy(card.gameObject);
        Destroy(_activeCard.gameObject);
        _activeCard = null;
        if (_cards.Count < 1) {
            Win();
            yield break;
        }

        if (_movementsUser >= _levels[_level].Movements) {
            Lose();
            yield break;
        }
        _blockInput = false;
    }

    private IEnumerator Fail(CardController card){
        card.Reveal();
        yield return new WaitForSeconds(1f);
        _activeCard.Hide();
        card.Hide();
        _activeCard = null;
        yield return new WaitForSeconds(0.5f);
        if (_movementsUser >= _levels[_level].Movements) {
            Lose();
            yield break;
        }
        _blockInput = false;
    }

    private void Win()
    {
        _level++;
        if(_level >= _levels.Count) {
            _level = 0;
        }
        PlayerPrefs.SetInt("Level", _level);
        PlayerPrefs.Save();
        _resetButton.SetActive(true);
    }

    private void Lose()
    {
        _resetButton.SetActive(true);
    }

}
