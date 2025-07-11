#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class MaterialReplacerWindow : EditorWindow
{
    private GameObject rootObject;
    private string nameFilter = "";

    [System.Serializable]
    private class MaterialWithProbability
    {
        public Material material;
        public float probability = 1f;
    }

    private List<MaterialWithProbability> materialList = new List<MaterialWithProbability>();
    private Vector2 scroll;

    [MenuItem("Tools/Probabilistic Material Replacer")]
    public static void ShowWindow()
    {
        GetWindow<MaterialReplacerWindow>("Material Replacer");
    }

    private void OnGUI()
    {
        GUILayout.Label("Probabilistic Material Replacer", EditorStyles.boldLabel);

        rootObject = (GameObject)EditorGUILayout.ObjectField("Root Object", rootObject, typeof(GameObject), true);
        nameFilter = EditorGUILayout.TextField("Name Filter", nameFilter);

        GUILayout.Space(10);
        GUILayout.Label("Materials with Probability", EditorStyles.boldLabel);

        scroll = EditorGUILayout.BeginScrollView(scroll, GUILayout.Height(150));
        for (int i = 0; i < materialList.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            materialList[i].material = (Material)EditorGUILayout.ObjectField(materialList[i].material, typeof(Material), false);
            materialList[i].probability = EditorGUILayout.FloatField(materialList[i].probability);
            if (GUILayout.Button("X", GUILayout.Width(20)))
            {
                materialList.RemoveAt(i);
                break;
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndScrollView();

        if (GUILayout.Button("Add Material"))
        {
            materialList.Add(new MaterialWithProbability());
        }

        GUILayout.Space(10);
        if (GUILayout.Button("Replace Materials"))
        {
            ReplaceMaterials();
        }
    }

    private void ReplaceMaterials()
    {
        if (rootObject == null || materialList.Count == 0)
        {
            Debug.LogWarning("Faltan parámetros.");
            return;
        }

        float totalProbability = 0f;
        foreach (var entry in materialList)
            totalProbability += Mathf.Max(0, entry.probability);

        if (totalProbability <= 0f)
        {
            Debug.LogWarning("Las probabilidades deben ser mayores que cero.");
            return;
        }

        int changed = 0;

        Transform[] allTransforms = rootObject.GetComponentsInChildren<Transform>(true);

        foreach (Transform t in allTransforms)
        {
            GameObject go = t.gameObject;

            // Filtro por nombre
            if (!string.IsNullOrEmpty(nameFilter) && !go.name.Contains(nameFilter))
                continue;

            // Si tiene LODGroup: cambiar todos sus hijos con Renderers al mismo material
            LODGroup lodGroup = go.GetComponent<LODGroup>();
            if (lodGroup != null)
            {
                Material chosen = GetRandomMaterial(totalProbability);
                Renderer[] childRenderers = go.GetComponentsInChildren<Renderer>(true);

                foreach (Renderer r in childRenderers)
                {
                    Material[] original = r.sharedMaterials;
                    Material[] replaced = new Material[original.Length];
                    for (int i = 0; i < replaced.Length; i++)
                        replaced[i] = chosen;

                    r.sharedMaterials = replaced;
                    EditorUtility.SetDirty(r);
                    changed++;
                }

                continue; // saltamos el resto para este objeto
            }

            // Si no tiene LODGroup: cambiar este objeto si tiene hijos y un renderer
            if (go.transform.childCount == 0)
                continue;

            Renderer renderer = go.GetComponent<Renderer>();
            if (renderer == null)
                continue;

            Material chosenMat = GetRandomMaterial(totalProbability);
            Material[] existing = renderer.sharedMaterials;
            Material[] newMats = new Material[existing.Length];
            for (int i = 0; i < newMats.Length; i++)
                newMats[i] = chosenMat;

            renderer.sharedMaterials = newMats;
            EditorUtility.SetDirty(renderer);
            changed++;
        }

        Debug.Log($"Materiales reemplazados en {changed} objeto(s).");
    }
    private Material GetRandomMaterial(float totalProbability)
    {
        float rand = Random.Range(0f, totalProbability);
        float cumulative = 0f;

        foreach (var entry in materialList)
        {
            cumulative += Mathf.Max(0, entry.probability);
            if (rand <= cumulative)
                return entry.material;
        }

        return null;
    }
}
#endif