using System;
using System.Collections.Generic;
using UnityEngine;


public static class IDFactory
{
    private static readonly HashSet<int> usedIds = new();
    
    public static int GetNewUniqueId(bool test = false)
    {
        if (test)
        {
            //usedIds.Clear();
            return -1;
        }
        int uniqueID;
        do
        {
            uniqueID = Guid.NewGuid().GetHashCode();
        } while (usedIds.Contains(uniqueID));
        usedIds.Add(uniqueID);
        return uniqueID;
    }

    public static void ReturnID(IUniquable uniqueIdHolder)
    {
        if (usedIds.Contains(uniqueIdHolder.Identity))
        {
            usedIds.Remove(uniqueIdHolder.Identity);
            Debug.Log($"Returning ID {uniqueIdHolder.Identity}");
            return;
        }
        
        Debug.LogWarning($"{uniqueIdHolder.Identity} is not used");
    }

    public static void RegisterID(IUniquable uniqueIdHolder)
    {
        if (usedIds.Contains(uniqueIdHolder.Identity))
        {
            Debug.LogWarning($"IDFactory already registered: {uniqueIdHolder.Identity} ");
            return;
        }
        usedIds.Add(uniqueIdHolder.Identity);
        
        Debug.Log($"Registering ID {uniqueIdHolder.Identity}");

    }
}
