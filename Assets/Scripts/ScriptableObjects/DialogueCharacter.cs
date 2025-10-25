using UnityEngine;

[ CreateAssetMenu ( fileName= "NewDialogueCharacter", menuName = "DialogueCharacter")]
public class DialogueCharacter : ScriptableObject
{
   
   [Header ("Character info")]
   [SerializeField]
   private string characterName;
   [SerializeField]
   private Sprite profilePhoto;

   public string Name => characterName;
   public Sprite ProfilePhoto => profilePhoto;

}
