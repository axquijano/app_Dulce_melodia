using System;
using UnityEngine;

[Serializable]
public class DialogueTurn
{
    [SerializeField]
    public DialogueCharacter Character;

    [SerializeField, TextArea (2,4)]
    private string dialogueLine = "";

    [SerializeField]
    private Sprite background;

    public string DialogueLine => dialogueLine;
    public Sprite Background => background;
}
