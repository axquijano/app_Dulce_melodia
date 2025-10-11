using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MiniGameManager : MonoBehaviour
{
    public TMP_Text puntuacionText;
    public float puntuacionValue;
    public TMP_Text puntuacionTextMAX;
    public float puntuacionValueMAX;
    public TMP_Text puntuacionFruitText;
    void Start()
    {
        puntuacionValue = 0;
        puntuacionValueMAX = PlayerPrefs.GetFloat("PuntuacionMaxima");
        puntuacionTextMAX.text = puntuacionValueMAX.ToString("0.00");
    }
    
    void Update()
    {
        puntuacionValue += Time.deltaTime;
        puntuacionText.text =  puntuacionValue.ToString("0.00");
        if(puntuacionValueMAX < puntuacionValue){
            PlayerPrefs.SetFloat("PuntuacionMaxima", puntuacionValue); 
            puntuacionTextMAX.text = puntuacionValueMAX.ToString("0.00");
        }
        
    }

    public void AddPuntuacion (float score){
        puntuacionValue += score;
        puntuacionFruitText.text =  score.ToString("0.00");
    }
}
