using System.Collections;
using System.Collections.Generic;
using Inventory.Pickups;
using Terra.Itemization.Abstracts;
using Terra.Itemization.Items;
using Terra.LootSystem;
using UnityEngine;

namespace Terra.Managers
{

    public class LootManager : MonoBehaviour
    {
        public static LootManager Instance;

        [SerializeField] private PickupContainer P_pickupContainer;
        [SerializeField] private ItemContainer P_itemContainer;
        [SerializeField] private LootTable lootTable;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        public void SpawnLoot()
        {
            List<Item> lootItems = lootTable.GetRandomItemsFromEachCategory();
            foreach (var item in lootItems)
            {
                Debug.Log($"Generated items: ");
            }

            List<Pickup> lootPickups = lootTable.GetRandomPickupsFromEachCategory();
            foreach (var pickup in lootPickups)
            {
                Debug.Log($"Generated pickups: ");
            }
        }

        public void SpawnRandomPickup(Transform pickupTransform)
        {
            PickupContainer pickupContainer = Instantiate(P_pickupContainer, pickupTransform);
            Pickup pickup = lootTable.GetRandomPickup();
            pickupContainer.Initialize(pickup);
        }
        
        public void SpawnRandomPickup(Vector3 pickupPosition)
        {
            PickupContainer pickupContainer = Instantiate(P_pickupContainer,pickupPosition, Quaternion.identity);
            Pickup pickup = lootTable.GetRandomPickup();
            pickupContainer.Initialize(pickup);
        }
    }
}