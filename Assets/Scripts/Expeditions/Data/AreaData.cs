using System;
using UnityEditor;
using UnityEngine;

// Not particularly useful as an object until more data is needed per area, so this is more for future infrastructure.
[CreateAssetMenu(fileName = "AreaData", menuName = "Expeditions/AreaData")]
public class AreaData : ScriptableObject
{
    public string TargetScene;

    private void OnValidate()
    {
#if UNITY_EDITOR
        if (string.IsNullOrEmpty(TargetScene))
        {
            TargetScene = name;
        }
        EditorUtility.SetDirty(this);
#endif
    }
}
