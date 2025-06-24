using UnityEngine;

namespace Terra.AI.Enemy
{
    public class EnemySpitter : EnemyRange
    {
        [SerializeField] private Transform _acidSpawnPoint;
        [SerializeField] AcidPool _acidPool;
        [SerializeField] private float _acidLifeDuration;
        [SerializeField] private int _acidDamage;
        
        protected override void OnDeath()
        {
            base.OnDeath();
            Instantiate(_acidPool, _acidSpawnPoint.position, _acidPool.transform.rotation).Init(_acidLifeDuration, _acidDamage);
        }
    }
}
