using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudsUpdater : MonoBehaviour
{
   

    public CustomRenderTexture customRenderTexture;

    void Update()
    {
        // Solo actualizar cuando el juego está en ejecución
        if (Application.isPlaying)
        {
            customRenderTexture.Update();
        }
    }
}
