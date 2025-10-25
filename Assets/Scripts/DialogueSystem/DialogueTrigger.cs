using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private DialogueRound dialogue;

   
    [ContextMenu ("Trigger Dialogue")]
    public void TriggerDialogue(){
        DialogueManager.dialogueManager.StartDialogue(dialogue);
    }

    private void onTriggerEnter2D(Collider2D collision) {
        if(collision.CompareTag("Player")){
            TriggerDialogue();
        }
    } 
}
