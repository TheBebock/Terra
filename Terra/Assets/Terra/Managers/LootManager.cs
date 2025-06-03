using Terra.Core.Generics;
using Terra.Itemization.Abstracts;
using Terra.Itemization.Items;
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
        
        public void SpawnRandomItem(Vector3 position)
        {
            ItemBase item = lootTable.GetRandomItem();
            SpawnItemContainer(item, position);
        }
        
        public void SpawnRandomItem(Transform itemTransform)
        {
            ItemBase item = lootTable.GetRandomItem();
            SpawnItemContainer(item, itemTransform);
        }

        public void SpawnItemContainer(ItemBase item, Transform itemTransform)
        {
            ItemContainer itemContainer = Instantiate(P_itemContainer, itemTransform);
            itemContainer.Initialize(item);
        }
        
        public void SpawnItemContainer(ItemBase item, Vector3 position)
        {
            ItemContainer itemContainer = Instantiate(P_itemContainer, position, Quaternion.identity);
            itemContainer.Initialize(item);
        }

        public void SpawnRandomPickup(Transform pickupTransform)
        {
            PickupBase pickup = lootTable.GetRandomPickup();
            if (pickup == null)
            {
                Debug.LogError($"{this}: Pickup could not be found");
                return;
            }
            SpawnPickupContainer(pickup, pickupTransform);
        }
        
        public void SpawnRandomPickup(Vector3 pickupPosition)
        {
            PickupBase pickup = lootTable.GetRandomPickup();
            if (pickup == null)
            {
                Debug.LogError($"{this}: Pickup could not be found");
                return;
            }
            SpawnPickupContainer(pickup, pickupPosition);
        }

        public void SpawnHealthPickup(Vector3 pickupPosition)
        {
            PickupBase pickup = lootTable.GetRandomHealthPickup();
            if (pickup == null)
            {
                Debug.LogError($"{this}: Pickup could not be found");
                return;
            }
            SpawnPickupContainer(pickup, pickupPosition);
        }
        
        public void SpawnAmmoPickup(Vector3 pickupPosition)
        {
            PickupBase pickup = lootTable.GetRandomAmmoPickup();
            if (pickup == null)
            {
                Debug.LogError($"{this}: Pickup could not be found");
                return;
            }
            SpawnPickupContainer(pickup, pickupPosition);
        }

        private void SpawnPickupContainer(PickupBase pickup, Vector3 pickupPosition)
        {
            PickupContainer pickupContainer = Instantiate(P_pickupContainer, pickupPosition, Quaternion.identity);
            pickupContainer.Initialize(pickup);
        }
        
        private void SpawnPickupContainer(PickupBase pickup, Transform pickupTransform)
        {
            PickupContainer pickupContainer = Instantiate(P_pickupContainer, pickupTransform);
            pickupContainer.Initialize(pickup);
        }
    }
}