using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public static class CollectionsExtenstions
{
    
    public static bool IsNullOrEmpty<T>(this IList<T> collection)
    {
        return collection == null || collection.Count == 0;
    }

    public static void AddRange<T>(this HashSet<T> hashSet, IEnumerable<T> range)
    {
        foreach (T obj in range)
            hashSet.Add(obj);
    }
    
    public static T GetRandomElement<T>(this IList collection)
    {
        if ((collection == null) || (collection.Count == 0))
        {
            return default(T);
        }

        int index = Random.Range(0, collection.Count);

        return collection[index] is T output ? output : default; 
    }
    
    public static T GetRandomElement<T>(this T[] collection)
    {
        if ((collection == null) || (collection.Length == 0))
        {
            return default(T);
        }

        int index = Random.Range(0, collection.Length);

        return collection[index];
    }

    public static int GetRandomElementIndex<T>(this IList<T> collection)
    {
        if ((collection == null) || (collection.Count == 0))
        {
            return -1;
        }

        return UnityEngine.Random.Range(0, collection.Count);
    }

    public static int GetRandomElementIndex<T>(this T[] collection)
    {
        if ((collection == null) || (collection.Length == 0))
        {
            return -1;
        }

        return UnityEngine.Random.Range(0, collection.Length);
    }
}
