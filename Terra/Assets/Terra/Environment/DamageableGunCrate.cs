using NaughtyAttributes;
using Terra.Itemization.Abstracts;
using Terra.Managers;
using UnityEngine;

namespace Terra.Environment
{
    public class DamageableGunCrate : DamageableObject
    {
        
        [SerializeField, MaxValue(-0.1f)] private float _spawnItemOffsetOnZ =-1f;
        [SerializeField, ReadOnly] ItemBase _itemToSpawn;
         private Vector3 SpawnLootOffset => new(
            transform.position.x, 
            transform.position.y, 
            transform.position.z + _spawnItemOffsetOnZ);

        public void Init(ItemBase item)
        {
            _itemToSpawn = item;
        }
        protected override void OnDeath()
        {
            base.OnDeath();
            
            LootManager.Instance?.SpawnItemContainer(_itemToSpawn, SpawnLootOffset);
        }
    }
}
