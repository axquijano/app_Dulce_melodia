using UnityEngine;

[CreateAssetMenu(fileName = "NewNotePair", menuName = "NotePair")]
public class NotePair : ScriptableObject
{
    public string noteName;
    public GameObject  note;
    public GameObject pictogram;
    public AudioClip noteSound;
    public int cardType;
}

/*
    0 - Si
    1 - La  
    2 - Sol
    3 - Fa
    4 - Mi  
    5 - Re
    6 - Do
 */
