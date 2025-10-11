using UnityEngine;

[CreateAssetMenu(fileName = "NewNoteFlute", menuName = "NoteFlute")]
public class NoteFlute : ScriptableObject
{
    public string noteName;        // Ejemplo: "Do", "Re", "Mi"
    public int noteType;           // 0 = Si, 1 = La, 2 = Sol, etc.
    public AudioClip noteSound;    // Sonido de la nota
    public bool[] holesPattern;    // Patrón de digitación (true = tapado)
    public Sprite noteSprite;    // Imagen representativa de la nota

    // Ejemplo de patrón:
    // 0 - Orificio de atras de la Flauta
    // [ 0 : true,  1: true, 2: true, 3: true, 4: false , 5: false, 6: false, 7: false]  → para nota "Sol"
}
