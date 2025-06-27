using System;
using System.Collections.Generic;
using Terra.Itemization;
using UnityEngine;

namespace Terra.ID
{
    public static class IDFactory
    {
        private static readonly HashSet<int> UsedIds = new();

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
                    UsedIds.Add(database.ItemDefinitions[i].Identity);
                }
            }
            else
            {
                Debug.LogError("Failed to load ItemDatabase.");
            }
        }

        public static void ClearUsedIds()
        {
            UsedIds.Clear();
        }

#endif

        /// <summary>
        /// Handles registering new and existing IDs
        /// </summary>
        /// <param name="uniqueIdHolder">Object with unique ID</param>
        public static void RegisterID(IUniqueable uniqueIdHolder)
        {
            if (uniqueIdHolder.Identity == Utils.Constants.DefaultID || uniqueIdHolder.Identity == 0)
            {
                int newID = GetNewUniqueId();
                uniqueIdHolder.SetID(newID);
                return;
            }
            
            if (CheckIsIDTaken(uniqueIdHolder.Identity))
            {
                Debug.LogWarning($"IDFactory already registered: {uniqueIdHolder.Identity} ");
                return;
            }
                 
            UsedIds.Add(uniqueIdHolder.Identity);

            Debug.Log($"Registering ID {uniqueIdHolder.Identity}");

        }

        /// <summary>
        /// Handles returning ID
        /// </summary>
        /// <param name="uniqueIdHolder">Object with unique ID</param>
        public static void ReturnID(IUniqueable uniqueIdHolder)
        {
            if (uniqueIdHolder.Identity == Utils.Constants.DefaultID)
            {
                Debug.LogWarning($"Trying to remove ID {Utils.Constants.DefaultID}, which is a default id");
                return;
            }
            
            if (UsedIds.Contains(uniqueIdHolder.Identity))
            {
                UsedIds.Remove(uniqueIdHolder.Identity);
                Debug.LogWarning($"Returning ID {uniqueIdHolder.Identity}");
                uniqueIdHolder.SetID(Utils.Constants.DefaultID);
                return;
            }

            Debug.LogWarning($"{uniqueIdHolder.Identity} is not used");
        }

        private static int GetNewUniqueId()
        {
            int uniqueID;
            do
            {
                uniqueID = Guid.NewGuid().GetHashCode();
            } while (CheckIsIDTaken(uniqueID));

            UsedIds.Add(uniqueID);
            return uniqueID;
        }
        
        private static bool CheckIsIDTaken(int id)
        {
            if (UsedIds.Contains(id))
            {
                return true;
            }

            return false;
        }
    }
}