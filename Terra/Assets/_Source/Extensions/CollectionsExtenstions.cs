using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CollectionsExtenstions
{
    public static void AddRange<T>(this HashSet<T> hashSet, IEnumerable<T> range)
    {
        foreach (T obj in range)
            hashSet.Add(obj);
    }
}
