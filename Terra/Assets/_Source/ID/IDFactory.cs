using System;
using System.Collections.Generic;
using Terra.Itemization;
using UnityEngine;

namespace Terra.ID
{
    public static class IDFactory
    {
        private static readonly HashSet<int> usedIds = new();

#if UNITY_EDITOR
        static IDFactory()
        {
            LoadDatabase();
        }

        static void LoadDatabase()
        {
            ItemsDatabase database = Resources.Load<ItemsDatabase>(nameof(ItemsDatabase));
            if (database)
            {
                Debug.Log($"Loaded {database.ItemDefinitions.Count} items");
                for (int i = 0; i < database.ItemDefinitions.Count; i++)
                {
                    usedIds.Add(database.ItemDefinitions[i].Identity);
                }
            }
            else
            {
                Debug.LogError("Failed to load ItemDatabase.");
            }
        }

#endif


        public static int GetNewUniqueId(bool clearUsedIDS = false)
        {
            if (clearUsedIDS)
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

        public static void ReturnID(IUniqueable uniqueIdHolder)
        {
            if (usedIds.Contains(uniqueIdHolder.Identity))
            {
                usedIds.Remove(uniqueIdHolder.Identity);
                Debug.Log($"Returning ID {uniqueIdHolder.Identity}");
                return;
            }

            Debug.LogWarning($"{uniqueIdHolder.Identity} is not used");
        }

        public static void RegisterID(IUniqueable uniqueIdHolder)
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
}