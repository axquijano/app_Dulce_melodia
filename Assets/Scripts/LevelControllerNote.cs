using UnityEngine;
using TMPro;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine.Events;

public class LevelControllerNote : MonoBehaviour
{
    [Serializable]
    public class LevelData {
        public int Columns;
        public int Rows;
        public int Difficulty;
        public int Movements;
        public int CardType = -1; /* Tipo de carta que se va a usar en este nivel, -1 es aleatorio */
    }


    [SerializeField]
    private CardControllerNote _cardPrefab;

    [Header ("UI Settings")]
    [SerializeField]
    private TMPro.TMP_Text _levelText;
    [SerializeField]
    private TMPro.TMP_Text _movesText;
    [SerializeField]
    private CardControllerNote _cardNotePrefab;  /* Carta que va a ser la principal y la cual deben dar match */

    [Header ("Level Settings")]
    [SerializeField]
    private List<LevelData> _levels = new List<LevelData>();

    private List<CardControllerNote> _cards = new List<CardControllerNote>();
    private int _movementsUser = 0;
    private CardControllerNote _activeCard = null; /* Carta que se está jugando actualmente */
    private int _level = 0;

    public event EventHandler OnLevelCompleted;
    public event EventHandler OnLevelFailed;

    public void StartLevel () {

        if (_levels[_level].Difficulty > _cardPrefab.MaxCardTypes) {
            Debug.Assert(false);
            _levels[_level].Difficulty = Math.Min(_levels[_level].Difficulty, _cardPrefab.MaxCardTypes);
        }

        Debug.Assert((_levels[_level].Rows * _levels[_level].Columns) % 2 == 0);
        _cards.ForEach(card => Destroy(card.gameObject));
        _cards.Clear();

        // El tipo de carta que se va a usar en este nivel
        if (_levels[_level].CardType < 0 || _levels[_level].CardType >= _cardPrefab.MaxCardTypes)
            _cardNotePrefab.CardType = UnityEngine.Random.Range(0, _cardPrefab.MaxCardTypes);
        else
            _cardNotePrefab.InitCardType(_levels[_level].CardType);
            Debug.Log($"Tipo de carta para este nivel: {_cardNotePrefab.CardType}");

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

       // Asegurarse que la carta principal esté en el juego
        if (!gameTypes.Contains(_cardNotePrefab.CardType)) {
            gameTypes[UnityEngine.Random.Range(0, gameTypes.Count)] = _cardNotePrefab.CardType;
        }


        for (int i = 0; i < _levels[_level].Rows; i++) {
            for (int j = 0; j < _levels[_level].Columns; j++) {
                Vector3 offset = new Vector3((_levels[_level].Columns - 2f) * _cardPrefab.CardSize, (_levels[_level].Rows - 2.2f) * _cardPrefab.CardSize, 0) * 0.5f;
                Vector3 position = new Vector3(j * _cardPrefab.CardSize, i * _cardPrefab.CardSize, 0);
                int type = gameTypes[UnityEngine.Random.Range(0, gameTypes.Count)];
                CardControllerNote card = Instantiate(_cardPrefab, position - offset, Quaternion.identity);
                card.InitCardType(type, true );
                gameTypes.Remove(type);
                /* Selección de carta */
                card.onClicked.AddListener(OnCardClicked);
                _cards.Add(card);
            }
        } 
        _movementsUser = 0;
        _levelText.text = $"Level: {_level}";
        _movesText.text = $"Moves: {_levels[_level].Movements }";
    }

    private void OnCardClicked(CardControllerNote card)
    {
        var prefab = _cardNotePrefab.prefabs.Find(x => x.cardType == card.CardType);
        Debug.Log($"Carta inicializada con tipo {card.CardType} y nombre {prefab.noteName}");
        card.SetHover(true);
        _cardNotePrefab.SetHover(true);

        if(card.CardType == _cardNotePrefab.CardType) {
            
            _movesText.text = "Correcto!";   
            if(GameManager.instance != null) {
                GameManager.instance.CompleteLevel(0);
            }
            OnLevelCompleted?.Invoke(this, EventArgs.Empty); 
        }   else {
            _movesText.text = "Intenta de nuevo";   
            OnLevelFailed?.Invoke(this, EventArgs.Empty);
        }
        return;
        
    }

    private IEnumerator SelectCard(CardControllerNote card)
    {
        card.Hide();
        return null;
       /*  yield return new WaitForSeconds(0.5f); */
  
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _level = PlayerPrefs.GetInt("Level", 0);
        StartLevel();
    }

    private IEnumerator Score (CardControllerNote card){
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
    }

    private IEnumerator Fail(CardControllerNote card){
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
    }

    private void Win()
    {
        _level++;
        if(_level >= _levels.Count) {
            _level = 0;
        }
        PlayerPrefs.SetInt("Level", _level);
        PlayerPrefs.Save();
      
    }

    private void Lose()
    {
       
    }

}
