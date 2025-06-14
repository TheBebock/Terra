using Cysharp.Threading.Tasks;
using Terra.Managers;
using UnityEngine;

namespace Terra.AI.Enemy
{
    public class EnemySplitter : EnemyMelee
    {
        [SerializeField] private EnemyBase _enemyToSpawn;
        [SerializeField] private int _amountToSpawn = 2;
        [SerializeField] private float _spawnDelay = 0.5f;
        protected override void SpawnLootOnDeath()
        {
            SpawnEnemies().Forget();
        }

        private async UniTaskVoid SpawnEnemies()
        {
            
            await UniTask.WaitForSeconds(_spawnDelay);
            
            if (!_enemyToSpawn)
            {
                Debug.LogError($"{gameObject.name} does not have enemies to spawn attached.");
                return;
            }
            for (int i = 0; i < _amountToSpawn; i++)
            {
                Vector3 randomOffset = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
                Vector3 spawnPosition = transform.position + randomOffset;
                WaveManager.Instance.SpawnEnemy(_enemyToSpawn, spawnPosition);
            }
        }
    }
}
