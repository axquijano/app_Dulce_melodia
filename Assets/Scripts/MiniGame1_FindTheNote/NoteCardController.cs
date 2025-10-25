using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class NoteCardController : MonoBehaviour
{
    [Header("Prefabs de notas y pictogramas")]
    [SerializeField] 
    public List<NotePair> AllNotePairs;

    //Cantidad total de tipos de cartas disponibles
    public int MaxCardTypes => AllNotePairs.Count; 

    [Header("Propiedades de la carta")]
    public float cardSize = 0.8f; 
    public int cardType = -1; // Tipo de carta (-1 = aleatorio)
    public bool isNoteCard = false; // true = nota, false = pictograma

    private Animator animator;

    // Evento que se dispara al hacer clic en la carta
    public UnityEvent<NoteCardController> onClicked;

    private AudioSource audioSource;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnMouseUpAsButton()
    {
        onClicked.Invoke(this); // notificar que esta carta fue clicada
    }

    // Método de prueba que revela y oculta la carta
    public void TestAnimation()
    {
        IEnumerator AnimationCoroutine() {
            Reveal();
            yield return new WaitForSeconds(1);
            Hide();
        }
        StartCoroutine(AnimationCoroutine());
    }

    // Revelar carta (animación de mostrarse)
    public void Reveal()
    {
        animator.SetBool("revealed", true);
    }

    // Ocultar carta (animación de esconderse)
    public void Hide()
    {
        animator.SetBool("revealed", false);
    }

    // Animación de hover cuando el ratón pasa encima
    public void SetHover(bool value)
    { 
        animator.SetBool("hover", value);
    }

    // Inicializar carta con tipo específico o aleatorio
    public void InitCardType(int type, bool asNoteCard = false) {
        cardType = type < 0 ? UnityEngine.Random.Range(0, AllNotePairs.Count) : type;
        var selectedPair = AllNotePairs.Find(x => x.cardType == cardType);
        GameObject prefabToUse = asNoteCard ? selectedPair.note : selectedPair.pictogram;
        if (prefabToUse != null) {
            Instantiate(prefabToUse, transform.position, Quaternion.identity, transform);
            Reveal(); // mostrar carta inicializada
        }
    }

    public int getTypeForName (string name) {
        var selectedPair = AllNotePairs.Find(x => x.noteName == name);
        if (selectedPair != null) {
            cardType = selectedPair.cardType;
            return cardType;
        }
        return -1;
    }

    public void reproduceSonido() {
        var selectedPair = AllNotePairs.Find(x => x.cardType == cardType);
        if (selectedPair != null && selectedPair.noteSound != null) {
            audioSource.PlayOneShot(selectedPair.noteSound);
        }
    }

    public void reproduceSonido(AudioClip clip) {
        if (clip != null) {
            audioSource.PlayOneShot(clip);
        }
    }

        public float GetSoundLength(AudioClip clip = null)
    {
        if (clip != null)
            return clip.length;

        var selectedPair = AllNotePairs.Find(x => x.cardType == cardType);
        if (selectedPair != null && selectedPair.noteSound != null)
            return selectedPair.noteSound.length;

        return 0f; // por si acaso no hay sonido
    }

}
