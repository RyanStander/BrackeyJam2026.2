using System;
using UnityEngine;

[Serializable]
public struct RegionMapIcons
{
    public Sprite Current, Past, Next, Unknown;

    public Sprite GetMapIcon(bool is_current, bool is_past, bool is_next)
    {
        if (is_current)
        {
            return Current;
        }
        else if (is_past)
        {
            return Past;
        }
        else if (is_next)
        {
            return Next;
        }
        return Unknown;
    }    
}
