using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudsUpdater : MonoBehaviour
{
   

    public CustomRenderTexture customRenderTexture;

    void Update()
    {
        // Solo actualizar cuando el juego est� en ejecuci�n
        if (Application.isPlaying)
        {
            customRenderTexture.Update();
        }
    }
}
