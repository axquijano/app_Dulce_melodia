using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine.Events;

public class BalloonNoteController : MonoBehaviour
{
    [SerializeField]
    public List<NotePair> prefabsBallon;

    public int MaxCardTypes => prefabsBallon.Count;
    public float balloonSize = 1f;
    public int balloonType = -1;
    public bool isNote = false;
    public float score = 0f;
    public UnityEvent<BalloonNoteController> onClicked;


    public void InitBalloonType(int type, bool isNote = false, float score = 0) {
        balloonType = type < 0 ? UnityEngine.Random.Range(0, prefabsBallon.Count) : type;
        var prefab = prefabsBallon.Find(x => x.cardType == balloonType);
        /* Debug.Log($"Carta inicializada con tipo {CardType} y nombre {prefab.noteName}"); */
        GameObject prefabToUse = isNote ? prefab.note : prefab.pictogram;
        this.score = score;
        if (prefabToUse != null) {
            Instantiate(prefabToUse, transform.position, Quaternion.identity, transform);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            FindFirstObjectByType<MiniGameManager>().AddPuntuacion(score);
        }
    }

    private void OnMouseUpAsButton()
    {
        onClicked.Invoke(this);
    }

}
