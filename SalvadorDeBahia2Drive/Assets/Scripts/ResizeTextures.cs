#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
public class ResizeTextures : EditorWindow
{
    int selectedSize = 512;
    int[] options = new int[] { 128, 256, 512, 1024, 2048, 4096 };
    string[] optionLabels;
    string folderPath = "Assets";

    [MenuItem("Tools/Resize Textures In Folder")]
    public static void ShowWindow()
    {
        GetWindow<ResizeTextures>("Resize Textures in Folder");
    }

    private void OnEnable()
    {
        optionLabels = new string[options.Length];
        for (int i = 0; i < options.Length; i++)
        {
            optionLabels[i] = options[i] + " px";
        }
    }

    void OnGUI()
    {
        GUILayout.Label("Redimensionar texturas en carpeta", EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();
        folderPath = EditorGUILayout.TextField("Carpeta base", folderPath);
        if (GUILayout.Button("...", GUILayout.Width(30)))
        {
            string selected = EditorUtility.OpenFolderPanel("Seleccionar carpeta", "Assets", "");
            if (!string.IsNullOrEmpty(selected))
            {
                // Convertir ruta absoluta a relativa
                if (selected.StartsWith(Application.dataPath))
                {
                    folderPath = "Assets" + selected.Substring(Application.dataPath.Length);
                }
            }
        }
        EditorGUILayout.EndHorizontal();

        selectedSize = EditorGUILayout.IntPopup("Tamaño máximo", selectedSize, optionLabels, options);

        if (GUILayout.Button("Redimensionar texturas"))
        {
            ResizeTexturesInFolder(folderPath, selectedSize);
        }
    }

    void ResizeTexturesInFolder(string folder, int maxSize)
    {
        string[] guids = AssetDatabase.FindAssets("t:Texture", new[] { folder });

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;

            if (importer != null)
            {
                importer.maxTextureSize = maxSize;
                importer.SaveAndReimport();
            }
        }

        Debug.Log($"✅ Se redimensionaron {guids.Length} texturas a un máximo de {maxSize}px en: {folder}");
    }
}

#endif