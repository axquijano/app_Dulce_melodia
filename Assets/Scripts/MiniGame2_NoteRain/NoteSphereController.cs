using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine.Events;

public class NoteSphereController : MonoBehaviour
{
    
    [SerializeField]
    public List<NotePair> prefabsSphere;

    public int MaxCardTypes => prefabsSphere.Count;
    public float sphereSize = 1f;
    public int sphereType = -1;
    public bool isCorrectNote = false;
    private Animator animator;
    private NoteRainManager manager;
    [SerializeField] 
    private Transform pictogramParent;

    public UnityEvent<NoteSphereController> onClicked;

    private void Awake() {
        animator = GetComponent<Animator>();
        manager = FindFirstObjectByType<NoteRainManager>();
    }

    public void InitSphereType(int type,  bool isCorrectNote) {
        sphereType = type < 0 ? UnityEngine.Random.Range(0, MaxCardTypes) : type;
        var prefab = prefabsSphere.Find(x => x.cardType == sphereType);
        GameObject prefabToUse =  prefab.pictogram;
        this.isCorrectNote = isCorrectNote;
        if (prefabToUse != null) {
            Instantiate(prefabToUse, pictogramParent.position, Quaternion.identity, pictogramParent);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collision detected with: " + collision.transform.name);
        if (collision.transform.CompareTag("Spikes") && isCorrectNote && !animator.GetBool("isCollected"))
        {
            manager?.AddMiss();
        }
    }

    private void OnMouseUpAsButton()
    {
        onClicked.Invoke(this);
    }

    public void SphereAnimate() {
        if(isCorrectNote) {
            SphereCollected();
            manager?.AddHit();
        } else {
            SphereExploded();
            manager?.AddMiss();
        }
    }

    public void SphereCollected() {
        if (animator != null) {
            animator.SetBool("isCollected", true);
        }
    }

    public void SphereExploded() {
        
        if (animator != null) {
            animator.SetBool("isExploding", true);
        }
    }

}
