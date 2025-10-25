using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    
    [Header ("Dialogue UI")]
    [SerializeField] private RectTransform dialogBox;
    [SerializeField] private Image characterPhoto;
    [SerializeField] private TMP_Text characterName;
    [SerializeField] private TMP_Text dialogArea;
    [SerializeField] private SpriteRenderer background;
    [SerializeField] private float newScale = 0.2559908f;

    public void ShowDialogBox (){
        dialogBox.gameObject.SetActive(true);
    }

    public void HideDialogBox(){
         dialogBox.gameObject.SetActive(false);
    }

    public void SetCharacterInfo(DialogueCharacter character){
        if( character == null) return;
        characterPhoto.sprite = character.ProfilePhoto;
        characterName.text = character.Name;
    }

    public void SetBackground(Sprite image){
        background.sprite = image;
        background.transform.localScale = new Vector3(newScale, newScale, 1);
    }

    public void ClearDialogArea () {
        dialogArea.text = "";
    }

    public void SetDialogArea(string text) {
        dialogArea.text = text;
    }

    public void AppendToDialogArea (char letter) {
        dialogArea.text += letter;
    }
}
