using NaughtyAttributes;
using Terra.AI.Enemy;
using Terra.Combat.Projectiles;
using UnityEngine;
using UnityEngine.Serialization;

namespace Terra.AI.Data
{
    [CreateAssetMenu(fileName = "BossEnemy_",menuName = "TheBebocks/AI/Data/BossEnemy")]
    public class BossEnemyData : EnemyData
    {
        [Min(1f)] public float dashModifier = 2f;
        [Min(1f)] public float attackCooldown = 5f;
        
        [BoxGroup("Cooldowns")][Min(1f)] public float pumpAttackCooldown = 10f;
        [BoxGroup("Cooldowns")][Min(1f)] public float spitAttackCooldown = 5f;
        
        [BoxGroup("PumpAttack")][SerializeField] public int pumpCyclesToPerform =3;
        [BoxGroup("PumpAttack")][SerializeField] public Vector3 onPumpScaleAdd = new(1, 0.5f, 0.5f);
        [BoxGroup("PumpAttack")][SerializeField] public AcidPool acidPoolPrefab;
        [BoxGroup("PumpAttack")][SerializeField] public float acidLifeDurationPerCycle = 10f;
        [BoxGroup("PumpAttack")][SerializeField] public int acidDamage = 1;
        
        [BoxGroup("RangeAttack")][SerializeField] public int howManyProjectiles = 7;
        [BoxGroup("RangeAttack")][SerializeField] public Vector2 directionOffSetRange = new Vector2(-0.2f, 0.2f);
        [BoxGroup("RangeAttack")][SerializeField] public Vector2 speedOffsetRange = new Vector2(0.75f, 1.25f);
        [BoxGroup("RangeAttack")][SerializeField] public BulletData enemyBulletData;

    }   
}
