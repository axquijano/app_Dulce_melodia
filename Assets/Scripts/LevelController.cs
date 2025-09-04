using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine.Events;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [SerializeField]
    private CardController _cardPrefab;
    private List<CardController> _cards = new List<CardController>();

    [SerializeField]
    private int _columns = 4;
    [SerializeField]
    private int _rows = 2;
    [SerializeField]
    private int _difficulty = 4;
    [SerializeField]
    private int _movements = 10;
    private int _movementsUser = 0;

    /* Carta que se está jugando actualmente */
    private CardController _activeCard = null;
    private bool _blockInput = true;

    public void StartLevel () {

        if (_difficulty > _cardPrefab.MaxCardTypes) {
            Debug.Assert(false);
            _difficulty = Math.Min(_difficulty, _cardPrefab.MaxCardTypes);
        }

        Debug.Assert((_rows * _columns) % 2 == 0);
        _cards.ForEach(card => Destroy(card.gameObject));
        _cards.Clear();

        /* Todas las cartas que se pueden crear */
        List<int> cardIndicesTypes = new List<int>();
        for (int i = 0; i < _cardPrefab.MaxCardTypes; i++) {
            cardIndicesTypes.Add(i);
        }

        /* Seleccion de las cartas que van a jugarse */
        List<int> gameTypes = new List<int>();
        for (int i = 0; i < _difficulty; i++) {
            int chosenType = cardIndicesTypes[UnityEngine.Random.Range(0, cardIndicesTypes.Count)];
            cardIndicesTypes.Remove(chosenType);
            gameTypes.Add(chosenType);
        }

        /* Duplicar las cartas */
        List<int> chosenTypes = new List<int>();
        for (int i = 0; i < (_rows * _columns) / 2; i++) {
            int chosenType = gameTypes[UnityEngine.Random.Range(0, gameTypes.Count)];
            chosenTypes.Add(chosenType);
            chosenTypes.Add(chosenType);
        }


        for (int i = 0; i < _rows; i++) {
            for (int j = 0; j < _columns; j++) {
                Vector3 offset = new Vector3((_columns - 2f) * _cardPrefab.CardSize, (_rows - 3.2f) * _cardPrefab.CardSize, 0) * 0.5f;
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
        _blockInput = false;
        if (_cards.Count < 1) {
            Win();
        }
    }

    private IEnumerator Fail(CardController card){
        card.Reveal();
        yield return new WaitForSeconds(1f);
        _activeCard.Hide();
        card.Hide();
        _activeCard = null;
        yield return new WaitForSeconds(0.5f);
        if (_movementsUser >= _movements) {
            Lose();
            yield break;
        }
        _blockInput = false;

    }

    private void Win()
    {
        Debug.Log("You win!");
    }

    private void Lose()
    {
        Debug.Log("You lose!");
    }

}
