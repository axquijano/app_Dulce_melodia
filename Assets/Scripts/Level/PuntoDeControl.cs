using UnityEngine;
using System;
using System.Collections;
using UnityEngine.SceneManagement;

public class PuntoDeControl : MonoBehaviour
{
    private Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(ActivateCheckpoint());
            
        }
    } 

    private IEnumerator ActivateCheckpoint()
    {
        animator.SetTrigger("activar");
        yield return new WaitForSeconds(3f);
        int levelsUnlocked = PlayerPrefs.GetInt("LevelsUnlocked", 0);
        PlayerPrefs.SetInt("LevelsUnlocked", levelsUnlocked + 1);
        PlayerPrefs.Save();
        SceneManager.LoadScene("LevelMenu");
    }
}
