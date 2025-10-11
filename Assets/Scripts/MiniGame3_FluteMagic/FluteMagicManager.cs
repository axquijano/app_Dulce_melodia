using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class FluteMagicManager : MonoBehaviour, IMinigameManager
{
    [Serializable]
    public class LevelSettings
    {
        public string[] targetNoteNames; 
    }

    [Header("Holes Settings")]
    public List<HoleFlute> holes = new List<HoleFlute>(); 

    [Header("Note Settings")]
    public List<NoteFlute> notes = new List<NoteFlute>(); 

    [Header("Text UI")]
    public Image imagePictogram;
    public GameObject FrontView;
    public GameObject BackView;
    public bool isFrontView = true;

    [Header("Level Settings")]
    [SerializeField] private List<LevelSettings> levels = new List<LevelSettings>();
    public NoteFlute currentNote;
    private int currentLevel = 0;


    public event EventHandler OnLevelCompleted;
    public event EventHandler OnLevelFailed;
   
     private void Start()
    {
        // Asigna listeners a los botones
        foreach (var hole in holes)
        {
            hole.holeButton.onClick.AddListener(() => OnHoleClicked(hole));
            hole.UpdateVisual();
        }

        currentLevel = PlayerPrefs.GetInt("LevelFluteMagic", 0);
        Debug.Log($"Nivel actual: {currentLevel}");

        LevelSettings settings = levels[currentLevel];
        currentNote = notes.Find(n => n.noteName == settings.targetNoteNames[0]);
        if (currentNote == null && notes.Count > 0)
        {
            currentNote = notes[0]; // Fallback si no se encuentra la nota
        }

        imagePictogram.sprite = currentNote != null ? currentNote.noteSprite : null;
        Debug.Log($"Nota objetivo: {currentNote.noteSprite}");

    }

    private void OnHoleClicked(HoleFlute hole)
    {
        Debug.Log($"Orificio {hole.position} clicado.");
        hole.ToggleHole();
    } 


    public void toggleVisualFlute() 
    {
        isFrontView = !isFrontView;
        FrontView.SetActive(isFrontView);
        BackView.SetActive(!isFrontView);
    }
    

    public void ValidateNote()
    {
        if (currentNote == null || currentNote.holesPattern == null) return;

        for (int i = 0; i < holes.Count; i++)
        {
            bool expected = currentNote.holesPattern[i];
            bool actual = holes[i].isActive;

            if (expected != actual)
            {
                OnLevelFailed?.Invoke(this, EventArgs.Empty);
                Debug.Log("❌ Digitación incorrecta para " + currentNote.noteName);
                return;
            }
        }

        currentLevel++;
        PlayerPrefs.SetInt("LevelFluteMagic", currentLevel);
        GameManager.instance.CompleteMiniGame(2);
        OnLevelCompleted?.Invoke(this, EventArgs.Empty);
        Debug.Log("✅ Digitación correcta para " + currentNote.noteName);
        // Aquí puedes reproducir el sonido:
        // AudioSource.PlayClipAtPoint(currentNote.noteSound, Camera.main.transform.position);
    }
}
