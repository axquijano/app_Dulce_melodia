using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine.Events;

public class CardController : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> prefabs;

    //lambda como getter 
    public int MaxCardTypes => prefabs.Count; 
    public float CardSize = 0.8f;
    public int CardType = -1;
    private Animator _animator;
    public UnityEvent<CardController> onClicked;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    void Start()
    {
        if (CardType  <  0)
        {
            CardType = UnityEngine.Random.Range(0, prefabs.Count);
        }
        Instantiate(prefabs[CardType], transform.position, quaternion.identity, transform);
        /* Reveal(); */
    }

    private void OnMouseUpAsButton()
    {
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

}