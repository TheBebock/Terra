using Terra.Core.Generics;
using Terra.Itemization.Abstracts;
using Terra.Itemization.Items;
using Terra.Itemization.Pickups;
using Terra.LootSystem;
using UnityEngine;
using UnityEngine.Serialization;

namespace Terra.Managers
{

    public class LootManager : MonoBehaviourSingleton<LootManager>
    {

        [FormerlySerializedAs("P_pickupContainer")] [SerializeField] private PickupContainer _pPickupContainer;
        [FormerlySerializedAs("P_itemContainer")] [SerializeField] private ItemContainer _pItemContainer;
        [FormerlySerializedAs("lootTable")] [SerializeField] private LootTable _lootTable = new();

        public LootTable LootTable => _lootTable;


        protected override void Awake()
        {
            base.Awake();
            _lootTable.Initialize();
        }
        
        public void SpawnRandomItem(Vector3 position)
        {
            ItemBase item = _lootTable.GetRandomItem();
            SpawnItemContainer(item, position);
        }
        
        public void SpawnRandomItem(Transform itemTransform)
        {
            ItemBase item = _lootTable.GetRandomItem();
            SpawnItemContainer(item, itemTransform);
        }

        public void SpawnItemContainer(ItemBase item, Transform itemTransform)
        {
            ItemContainer itemContainer = Instantiate(_pItemContainer, itemTransform);
            itemContainer.Initialize(item);
        }
        
        public void SpawnItemContainer(ItemBase item, Vector3 position)
        {
            ItemContainer itemContainer = Instantiate(_pItemContainer, position, Quaternion.identity);
            itemContainer.Initialize(item);
        }

        public void SpawnRandomPickup(Transform pickupTransform)
        {
            PickupBase pickup = _lootTable.GetRandomPickup();
            if (pickup == null)
            {
                Debug.LogError($"{this}: Pickup could not be found");
                return;
            }
            SpawnPickupContainer(pickup, pickupTransform);
        }
        
        public void SpawnRandomPickup(Vector3 pickupPosition)
        {
            PickupBase pickup = _lootTable.GetRandomPickup();
            if (pickup == null)
            {
                Debug.LogError($"{this}: Pickup could not be found");
                return;
            }
            SpawnPickupContainer(pickup, pickupPosition);
        }

        public void SpawnHealthPickup(Vector3 pickupPosition)
        {
            PickupBase pickup = _lootTable.GetRandomHealthPickup();
            if (pickup == null)
            {
                Debug.LogError($"{this}: Pickup could not be found");
                return;
            }
            SpawnPickupContainer(pickup, pickupPosition);
        }
        
        public void SpawnAmmoPickup(Vector3 pickupPosition)
        {
            PickupBase pickup = _lootTable.GetRandomAmmoPickup();
            if (pickup == null)
            {
                Debug.LogError($"{this}: Pickup could not be found");
                return;
            }
            SpawnPickupContainer(pickup, pickupPosition);
        }

        public void SpawnCrystalPickup(Vector3 pickupPosition)
        {
            PickupBase pickup = _lootTable.GetRandomCrystalPickup();
            if (pickup == null)
            {
                Debug.LogError($"{this}: Pickup could not be found");
                return;
            }
            SpawnPickupContainer(pickup, pickupPosition);
        }

        private void SpawnPickupContainer(PickupBase pickup, Vector3 pickupPosition)
        {
            PickupContainer pickupContainer = Instantiate(_pPickupContainer, pickupPosition, Quaternion.identity);
            pickupContainer.Initialize(pickup);
        }
        
        private void SpawnPickupContainer(PickupBase pickup, Transform pickupTransform)
        {
            PickupContainer pickupContainer = Instantiate(_pPickupContainer, pickupTransform);
            pickupContainer.Initialize(pickup);
        }
    }
}