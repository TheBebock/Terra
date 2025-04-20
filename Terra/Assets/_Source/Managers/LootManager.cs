using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Terra.Core.Generics;
using Terra.Itemization.Abstracts;
using Terra.Itemization.Items;
using Terra.Itemization.Items.Definitions;
using Terra.Itemization.Pickups;
using Terra.LootSystem;
using UnityEngine;

namespace Terra.Managers
{

    public class LootManager : MonoBehaviourSingleton<LootManager>
    {

        [SerializeField] private PickupContainer P_pickupContainer;
        [SerializeField] private ItemContainer P_itemContainer;
        [SerializeField] private LootTable lootTable = new();

        public LootTable LootTable => lootTable;


        protected override void Awake()
        {
            base.Awake();
            lootTable.Initialize();
        }


        public void SpawnLoot()
        {
            List<ItemBase> lootItems = lootTable.GetRandomItemsFromEachCategory();
            foreach (var item in lootItems)
            {
                Debug.Log($"Generated items: {item.ItemName}");
            }

            List<PickupBase> lootPickups = lootTable.GetRandomPickupsFromEachCategory();
            foreach (var pickup in lootPickups)
            {
                Debug.Log($"Generated pickups: ");
            }
        }

        public void SpawnItem(ItemBase item, Vector3 position)
        {
            ItemContainer itemContainer = Instantiate(P_itemContainer, position, Quaternion.identity);
            itemContainer.Initialize(item);
        }

        public void SpawnRandomItem(Vector3 position)
        {
            ItemContainer itemContainer = Instantiate(P_itemContainer, position, Quaternion.identity);
            ItemBase item = lootTable.GetRandomItem();
            itemContainer.Initialize(item);
        }
        
        public void SpawnRandomItem(Transform itemTransform)
        {
            ItemContainer itemContainer = Instantiate(P_itemContainer, itemTransform);
            ItemBase item = lootTable.GetRandomItem();
            itemContainer.Initialize(item);
        }
        
        public void SpawnRandomPickup(Transform pickupTransform)
        {
            PickupContainer pickupContainer = Instantiate(P_pickupContainer, pickupTransform);
            PickupBase pickup = lootTable.GetRandomPickup();
            pickupContainer.Initialize(pickup);
        }
        
        public void SpawnRandomPickup(Vector3 pickupPosition)
        {
            PickupContainer pickupContainer = Instantiate(P_pickupContainer,pickupPosition, Quaternion.identity);
            PickupBase pickup = lootTable.GetRandomPickup();
            pickupContainer.Initialize(pickup);
        }


    }
}