using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu (fileName = "NewDialogueRound", menuName = "DialogueRound")]
public class DialogueRound : ScriptableObject
{
    [SerializeField]
    private List<DialogueTurn> dialogueTurnsList;

    public List <DialogueTurn> DialogueTurnsList => dialogueTurnsList;


}
