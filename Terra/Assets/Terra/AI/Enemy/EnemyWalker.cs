using JetBrains.Annotations;
using Terra.Enums;
using Terra.Environment;
using UnityEngine;

namespace Terra.AI.Enemy
{
    public class EnemyWalker : EnemyMelee
    {
        [SerializeField] DamageableStatue _walkerStatue;
        [SerializeField] private Transform _statueSpawnPointLeft;
        [SerializeField] private Transform _statueSpawnPointRight;
        protected override void SpawnLootOnDeath()
        {
            //Do nothing
        }

        protected override void OnDeath()
        {
            base.OnDeath();
            _enemyCollider.enabled = true;
        }

        protected override void BeforeDeletion()
        {
            base.BeforeDeletion();
            SpawnStatue();
        }

        [UsedImplicitly]
        private void SpawnStatue()
        {
            switch (CurrentDirection)
            {
                case FacingDirection.Left:
                    Instantiate(_walkerStatue, _statueSpawnPointLeft.position, Quaternion.identity).Init(CurrentDirection, _deathSFX);
                    break;
                case FacingDirection.Right:
                    Instantiate(_walkerStatue, _statueSpawnPointRight.position, Quaternion.identity).Init(CurrentDirection, _deathSFX);
                    break;
            }
        }
    }
}
