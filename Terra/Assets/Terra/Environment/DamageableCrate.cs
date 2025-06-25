using NaughtyAttributes;
using Terra.Managers;
using UnityEngine;

namespace Terra.Environment
{
    public class DamageableCrate : DamageableObject
    {
        
        [SerializeField, MaxValue(-0.1f)] private float _spawnItemOffsetOnZ =-1f;
        private Vector3 SpawnLootOffset => new(
            transform.position.x, 
            transform.position.y, 
            transform.position.z + _spawnItemOffsetOnZ);
        protected override void OnDeath()
        {
            base.OnDeath();
            
            float random = Random.Range(0f, 100f);

            if (random < 60)
            {
                LootManager.Instance.SpawnHealthPickup(SpawnLootOffset);
            }
            else
            {
                LootManager.Instance.SpawnAmmoPickup(SpawnLootOffset);
            }
        }
    }
}
