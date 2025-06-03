using Terra.Managers;

namespace Terra.Environment
{
    public class DamageableCrate : DamageableObject
    {
        protected override void OnDeath()
        {
            base.OnDeath();
            
            float random = UnityEngine.Random.Range(0f, 100f);

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
