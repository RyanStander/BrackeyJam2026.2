using UnityEditor;
using UnityEngine;

// Initial version lineal
[CreateAssetMenu(fileName = "RegionData", menuName = "Expeditions/RegionData")]
public class RegionData : ScriptableObject
{
    public string RegionName;
    public string RegionDestination;
    public string StickyNoteText;

    public AreaData[] AreasLinear;

    public RegionMapIcons Icons;

    private void OnValidate()
    {
#if UNITY_EDITOR
        if (string.IsNullOrEmpty(RegionName))
        {
            RegionName = name;
        }
        EditorUtility.SetDirty(this);
#endif
    }
}
