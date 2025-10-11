using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[Serializable]
public class HoleFlute
{
    public int position;                // Índice o posición en la flauta
    public Button holeButton;           // Referencia al botón UI
    public Image holeSprite;            // Imagen circular del orificio
    public Color activeColor = Color.black;   // Color cuando está tapado
    public Color inactiveColor = Color.white; // Color cuando está libre
    public bool isActive;               // Estado actual (tapado o no)
    
    // ✅ Actualiza el color visual según el estado actual
    public void UpdateVisual()
    {
        Color currentColor = isActive ? activeColor : inactiveColor;
        Debug.Log($"Hole at position {position} is now {(isActive ? "Active" : "Inactive")} color: {currentColor}");
        if (holeSprite != null)
            holeSprite.color = currentColor;
    }

    // ✅ Cambia el estado al hacer click
    public void ToggleHole()
    {
        isActive = !isActive;
        Debug.Log($"Hole at position {position} toggled to {(isActive ? "Active" : "Inactive")}");
        UpdateVisual();
    }
}
