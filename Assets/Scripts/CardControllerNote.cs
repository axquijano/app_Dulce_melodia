using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine.Events;

public class CardControllerNote : MonoBehaviour
{
    [SerializeField]
    /* private List<NotePair> prefabsNotePair; */
    public List<NotePair> prefabs;

    //lambda como getter 
    public int MaxCardTypes => prefabs.Count; 
    public float CardSize = 0.8f;
    public int CardType = -1;
    public bool isNote = false;
    private Animator _animator;
    public UnityEvent<CardControllerNote> onClicked;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
       
    }

    private void OnMouseUpAsButton()
    {
        Debug.Log($"Carta clickeada de tipo {CardType}");
        onClicked.Invoke(this);
    }

    public void TestAnimation()
    {
        IEnumerator AnimationCoroutine() {
            Reveal();
            yield return new WaitForSeconds(1);
            Hide();
        }

        StartCoroutine(AnimationCoroutine());
    }

    public void Reveal (){
        _animator.SetBool("revealed", true);
    }

    public void Hide (){
        _animator.SetBool("revealed", false);
    }

    public void SetHover(bool value)
    { 
        _animator.SetBool("hover", value);
    }

    public void InitCardType(int type, bool isNote = false) {
        CardType = type < 0 ? UnityEngine.Random.Range(0, prefabs.Count) : type;
        var prefab = prefabs.Find(x => x.cardType == CardType);
        /* Debug.Log($"Carta inicializada con tipo {CardType} y nombre {prefab.noteName}"); */
        GameObject prefabToUse = isNote ? prefab.note : prefab.pictogram;
        if (prefabToUse != null) {
            Instantiate(prefabToUse, transform.position, Quaternion.identity, transform);
            Reveal();
        }
    }

}