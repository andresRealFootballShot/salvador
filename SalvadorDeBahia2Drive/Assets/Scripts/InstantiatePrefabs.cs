#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

using UnityEditor.SceneManagement;

using System.Collections.Generic;
public class InstantiatePrefabs : EditorWindow
{
    // Prefab a instanciar
    public List<GameObject> prefabList = new List<GameObject>();

    // Objeto root que contiene los hijos
    public Transform root;
    public Transform newInstancesRoot;
    // Crear el menú para abrir la ventana
    [MenuItem("Tools/Prefab Instancer")]
    public static void ShowWindow()
    {
        // Crea una ventana de tipo PrefabInstancerWindow
        GetWindow<InstantiatePrefabs>("Prefab Instancer");
    }

    // Función para dibujar el contenido de la ventana
    private void OnGUI()
    {
        // Campo para asignar el prefab
        GUILayout.Label("Prefab List", EditorStyles.boldLabel);

        // Dibujar la lista de prefabs (GameObjects)
        for (int i = 0; i < prefabList.Count; i++)
        {
            GUILayout.BeginHorizontal();

            // Crear un ObjectField para cada prefab en la lista
            prefabList[i] = (GameObject)EditorGUILayout.ObjectField(prefabList[i], typeof(GameObject), false);

            // Botón para eliminar un prefab de la lista
            if (GUILayout.Button("Remove", GUILayout.Width(70)))
            {
                prefabList.RemoveAt(i);
            }

            GUILayout.EndHorizontal();
        }
        if (GUILayout.Button("Add Prefab"))
        {
            prefabList.Add(null); // Añadir un nuevo espacio vacío para un prefab
        }
        // Campo para asignar el root (el padre de los objetos)
        root = (Transform)EditorGUILayout.ObjectField("Root", root, typeof(Transform), true);
        newInstancesRoot = (Transform)EditorGUILayout.ObjectField("New Instances Root", newInstancesRoot, typeof(Transform), true);

        // Botón para instanciar el prefab
        if (GUILayout.Button("Instanciar Prefabs"))
        {
            instantiatePrefabs();
        }
    }

    // Función para instanciar los prefabs en las posiciones de los hijos
    private void instantiatePrefabs()
    {
        // Verificar si se asignó un prefab y root
        if (prefabList != null && root != null)
        {
            // Iterar sobre cada hijo del root
            foreach (Transform child in root)
            {
                GameObject prefab = FindGameObjectByName(prefabList,child.name);
                if (prefab == null) continue;
                // Instanciar el prefab en modo de edición
                GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);

                // Colocar la instancia en la misma posición y rotación que el hijo
                instance.transform.position = child.position;
                instance.transform.rotation = child.rotation;
                instance.transform.parent = newInstancesRoot;
                // Registrar la instancia en la escena para que sea persistente
                Undo.RegisterCreatedObjectUndo(instance, "Instantiate Prefab");
            }

            // Refrescar la escena para que los cambios sean visibles
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }
        else
        {
            // Mensaje de advertencia si falta el prefab o el root
            Debug.LogWarning("Asigna un prefab y un objeto root antes de instanciar.");
        }
    }
    public GameObject FindGameObjectByName(List<GameObject> gameObjects, string searchText)
    {
        foreach (GameObject go in gameObjects)
        {
            if (go != null && searchText.ToLower().Contains(go.name.ToLower()))
            {
                return go; // Retorna el primer GameObject que coincida
            }
        }
        return null; // Si no encuentra ninguno, retorna null
    }
}
#endif