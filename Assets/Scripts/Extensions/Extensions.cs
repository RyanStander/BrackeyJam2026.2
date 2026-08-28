using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    // Fisher-Yates Styled List Shuffle
    public static void Shuffle<T>(this IList<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int rand = Random.Range(0, i + 1); // Random index from 0 to i
            (list[rand], list[i]) = (list[i], list[rand]);
        }
    }
}