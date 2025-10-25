using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private DialogueUI dialogueUI;
    [SerializeField] private float typingSpeed = 0.05f;
    [SerializeField] private AudioSource typingAudioSource;

    [SerializeField] private Button btnContinue;
    private bool buttonClicked = false;

    public static DialogueManager dialogueManager;
    private Queue <DialogueTurn> dialogueTurnsQueue;
    public bool IsDialogInProgress { get; private set;} = false ;

    private void Awake()
    {
        // Implement singleton pattern
        if (dialogueManager == null)
        {
            dialogueManager = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return; 
        }

        if (dialogueUI != null ) {
            btnContinue.onClick.AddListener(OnButtonClicked);
            btnContinue.interactable = false;
            dialogueUI.HideDialogBox();
        } 
    }


    public void StartDialogue(DialogueRound dialogue){
        if (IsDialogInProgress) return ;
        if (dialogueTurnsQueue != null && dialogueTurnsQueue.Count == 0) return; // ya terminó
        IsDialogInProgress = true;
        dialogueTurnsQueue = new Queue<DialogueTurn>(dialogue.DialogueTurnsList);
        StartCoroutine(DialogueCoroutine());
    }

    private IEnumerator DialogueCoroutine () {
        dialogueUI.ShowDialogBox();
        while( dialogueTurnsQueue.Count > 0 ){
            var currentTurn = dialogueTurnsQueue.Dequeue();
            dialogueUI.SetCharacterInfo(currentTurn.Character);
            dialogueUI.ClearDialogArea();
            yield return StartCoroutine(TypeSentence(currentTurn));
            yield return new WaitUntil(() => buttonClicked);
            yield return null;
            buttonClicked = false;
        }
        
        IsDialogInProgress = false;
        dialogueUI.HideDialogBox();
    }

    private IEnumerator TypeSentence(DialogueTurn dialogTurn){
        var typingWaitSeconds = new WaitForSeconds(typingSpeed);
        Sprite spritebackground = dialogTurn.Background;
        if(spritebackground) dialogueUI.SetBackground(spritebackground);
        foreach( char letter in dialogTurn.DialogueLine.ToCharArray()){
            dialogueUI.AppendToDialogArea(letter);
            if(!char.IsWhiteSpace(letter)) typingAudioSource.Play();
            yield return typingWaitSeconds;
        }
        btnContinue.interactable = true;
    }

    private void OnButtonClicked (){
        if(btnContinue.interactable){
            buttonClicked = true;
            btnContinue.interactable = false;
        }
    }

    public bool IsDialogFinished()
    {
        return (dialogueTurnsQueue == null || dialogueTurnsQueue.Count == 0) && !IsDialogInProgress;
    }

    public void SkipDialog()
    {
        StopAllCoroutines();  // Detiene animaciones y tipeo
        dialogueTurnsQueue?.Clear();  // Vacía la cola de turnos
        IsDialogInProgress = false;   // Marca el diálogo como finalizado
        dialogueUI.HideDialogBox();   // Oculta el panel del diálogo
        btnContinue.interactable = false; // Desactiva el botón continuar
    }

    
}
