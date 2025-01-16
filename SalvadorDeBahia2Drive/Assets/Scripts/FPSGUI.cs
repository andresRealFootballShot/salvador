using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSGUI : MonoBehaviour
{
    private float deltaTime = 0.0f;

    void Update()
    {
        // Calcula el tiempo entre frames
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
    }

    void OnGUI()
    {
        // Calcula los FPS
        float fps = 1.0f / deltaTime;
        string text = $"FPS: {fps:0.}";

        // Configura el estilo de texto
        GUIStyle style = new GUIStyle();
        Rect rect = new Rect(10, 10, 200, 50); // Posición en la pantalla
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = 24; // Tamaño de la fuente
        style.normal.textColor = Color.white;

        // Dibuja el texto en pantalla
        GUI.Label(rect, text, style);
    }
}
