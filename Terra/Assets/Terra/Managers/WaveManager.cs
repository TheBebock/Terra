﻿using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using Terra.AI.Enemy;
using Terra.Core.Generics;
using Terra.EventsSystem;
using Terra.EventsSystem.Events;
using Terra.Extensions;
using Terra.Interfaces;
using UnityEngine;
using Random = UnityEngine.Random;


namespace Terra.Managers
{

    public class WaveManager : MonoBehaviourSingleton<WaveManager>, IAttachListeners
    {
        [Serializable]
        internal struct EnemySpawnData
        {
            public EnemyBase enemy;
            public int spawnValue;
        }

        
        [Header("Is Enabled")]
        [SerializeField] private bool _isEnabled = true;
        
        [SerializeField] private List<EnemySpawnData> _enemies;
        [SerializeField] private List<EnemySpawnData> _firstFloorEnemies;

        [BoxGroup("Boss Settings")] [SerializeField]
        private int _waveToSpawnBoss;
        [BoxGroup("Boss Settings")] [SerializeField] EnemyBoss _bossPrefab;
        
        [Header("Waves Settings")] 
        [SerializeField] private float _delayBeforeFirstWave = 5f;
        [SerializeField] private float _timeBetweenWaves = 10f;
        [SerializeField] private int _startingWavesToSpawn = 3;
        [SerializeField] private float _wavesGainPerLevel = 0.33f;
        [SerializeField] private int _startingSpawnPoints = 100;
        [SerializeField] private int _spawnPointsGain = 50;
        [SerializeField] private float _enemiesToSpawnPerWave = 1f;
        [SerializeField] private float _enemiesPerWaveGain = 0.5f;
        [SerializeField] private float _spawnTimeInterval = 0.2f;

        [Header("Spawn Area")]
        [SerializeField] private LayerMask _raycastLayerMask;
        [SerializeField] private Vector2 _spawnMin;
        [SerializeField] private Vector2 _spawnMax;
        
        [Foldout("Debug"), ReadOnly][SerializeField] private bool _isPaused;
        [Foldout("Debug"), ReadOnly][SerializeField] private int _currentSpawnPoints;
        [Foldout("Debug"), ReadOnly][SerializeField] private int _currentWaveIndex;
        [Foldout("Debug"), ReadOnly][SerializeField] private int _wavesToSpawn;
        [Foldout("Debug"), ReadOnly][SerializeField] private int _currentLevel;
        [Foldout("Debug"), ReadOnly][SerializeField] private int _currentActiveEnemies;

        private CancellationTokenSource _waveCancellationTokenSource;
        private CancellationTokenSource _linkedCts;
        private LayerMask _groundLayer;
        private LevelIncreasedEvent _levelIncreasedEvent;
        public int CurrentLevel => _currentLevel;
        
        protected override void Awake()
        {
            base.Awake();
            _groundLayer = LayerMask.NameToLayer("Ground");
            EventsAPI.Register<StartOfNewFloorEvent>(OnStartOfNewFloor);
            EventsAPI.Register<OnBossDiedEvent>(OnBossDiedEvent);
            
            _levelIncreasedEvent = new LevelIncreasedEvent();
        }

        
        public void AttachListeners()
        {
           
            if (!TimeManager.Instance) return;

            TimeManager.Instance.OnGamePaused += PauseWaves;
            TimeManager.Instance.OnGameResumed += ResumeWaves;
        }

        public void StartWaves()
        {
            
#if UNITY_EDITOR
            
            if(!_isEnabled)return;
#endif
            
            StopWaves();
            
            _waveCancellationTokenSource = new CancellationTokenSource(); 
            _linkedCts = CancellationTokenSource.CreateLinkedTokenSource(
                _waveCancellationTokenSource.Token, 
                CancellationToken
            );
            _ = StartSpawningWaves(_linkedCts.Token);
        }

        private void OnBossDiedEvent(ref OnBossDiedEvent ev)
        {
            StopWaves();
        }
        
        private void StopWaves()
        {

            if (_waveCancellationTokenSource != null && !_waveCancellationTokenSource.IsCancellationRequested)
            {
                _waveCancellationTokenSource?.Cancel();
            }
            
            _linkedCts?.Dispose();
            _waveCancellationTokenSource?.Dispose();
        }
        

        private void OnStartOfNewFloor(ref StartOfNewFloorEvent dummy)
        {
            _currentLevel++;
            _levelIncreasedEvent.level = _currentLevel;
            EventsAPI.Invoke(ref _levelIncreasedEvent);
            
            TrySpawningBoss();
        }

        public void PauseWaves() => _isPaused = true;
        
        public void ResumeWaves() => _isPaused = false;
        
        private async UniTask StartSpawningWaves(CancellationToken token)
        {

            
            await UniTask.WaitForSeconds(_delayBeforeFirstWave, cancellationToken: token);
            
            _wavesToSpawn = _startingWavesToSpawn + Mathf.RoundToInt(_currentLevel * _wavesGainPerLevel);
            _currentWaveIndex = 0;

            while (_currentWaveIndex < _wavesToSpawn)
            {
                await UniTask.WaitWhile(() => _isPaused || Time.timeScale == 0, cancellationToken: token);
                
                _currentWaveIndex++;
                
                await HandleWaveSpawning(token);

                float elapsed = 0f;
                while (elapsed < _timeBetweenWaves && _currentActiveEnemies > 0)
                {
                    if (Time.timeScale > 0)
                        elapsed += Time.deltaTime;

                    await UniTask.Yield(PlayerLoopTiming.Update, token);
                }
            }

            while (_currentActiveEnemies > 0)
            {
                await UniTask.Yield(PlayerLoopTiming.Update, token);
            }
            
            OnLevelEnd();
        }

        private void TrySpawningBoss()
        {
            if(_currentLevel != _waveToSpawnBoss) return;
            
            EnemyBoss enemy = Instantiate(_bossPrefab, _bossPrefab.transform.position, Quaternion.identity);
            //NOTE: Increasing active enemies here and not decreasing it on boss death, make the WaveEnded event to never be invoked
            // It is not supposed to get invoked on floor with boss
            _currentActiveEnemies++;
            OnBossSpawnedEvent ev = new OnBossSpawnedEvent();
            ev.bossHp = enemy.HealthController.MaxHealth;
            EventsAPI.Invoke(ref ev);
            
        }
        private async UniTask HandleWaveSpawning(CancellationToken token)
        {
            _currentSpawnPoints = _startingSpawnPoints + _spawnPointsGain * _currentWaveIndex;
            int numberOfEnemies = Mathf.CeilToInt(_enemiesToSpawnPerWave + _currentWaveIndex * _enemiesPerWaveGain); 
            
            for (int i = 0; i < numberOfEnemies; i++)
            {
                await UniTask.WaitWhile(() => _isPaused || Time.timeScale == 0, cancellationToken: token);

                SpawnEnemy(GetEnemyBasedOnSpawnValue());
                
                float elapsed = 0f;
                while (elapsed < _spawnTimeInterval)
                {
                    await UniTask.WaitWhile(() => _isPaused || Time.timeScale == 0, cancellationToken: token);
                    elapsed += Time.deltaTime;
                    await UniTask.Yield(PlayerLoopTiming.Update, token);
                }
            }
        }
        
        private EnemyBase GetEnemyBasedOnSpawnValue()
        {
            // Default enemy to spawn
            List<EnemySpawnData> possibleEnemies = new() { _enemies[0] };

            if(_currentLevel == 1)
            {
                for (int i = 1; i < _firstFloorEnemies.Count; i++)
                {
                    if (_firstFloorEnemies[i].spawnValue > _currentSpawnPoints) continue;
                    possibleEnemies.Add(_firstFloorEnemies[i]);
                }
            }
            else
            {
                for (int i = 1; i < _enemies.Count; i++)
                {
                    if(_enemies[i].spawnValue > _currentSpawnPoints) continue;   
                    possibleEnemies.Add(_enemies[i]);
                }
            }

            EnemySpawnData enemyData = possibleEnemies.GetRandomElement<EnemySpawnData>();
            _currentSpawnPoints -= enemyData.spawnValue;
            return enemyData.enemy;
        }

        private void SpawnEnemy(EnemyBase enemyPrefab)
        {
            Vector3 spawnPosition = GetRandomSpawnPosition();
            EnemyBase enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

            _currentActiveEnemies++;
            enemy.onDestroy += () => { _currentActiveEnemies--; };
        }
        
        public void SpawnEnemy(EnemyBase enemyPrefab, Vector3 position)
        {
            EnemyBase enemy = Instantiate(enemyPrefab, position, Quaternion.identity);

            _currentActiveEnemies++;
            enemy.HealthController.OnDeath += () => { _currentActiveEnemies--; };
        }

        private void OnLevelEnd()
        {
            if (_currentActiveEnemies <= 0)
            {
                EventsAPI.Invoke<WaveEndedEvent>();
                Debug.Log($"{gameObject.name}: OnWaveEnded event raised");
            }
        }

        private Vector3 GetRandomSpawnPosition()
        {
            int atempts = 8;
            float x = Random.Range(_spawnMin.x, _spawnMax.x);
            float z = Random.Range(_spawnMin.y, _spawnMax.y);
            
            while (atempts > 0)
            {
                Vector3 rayOrigin = new Vector3(x, 120f, z);

                if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, 100f, _raycastLayerMask))
                {
                    if (hit.transform.gameObject.layer == _groundLayer)
                    {
                        return hit.point;
                    }
                }
                atempts--;
                                
                x = Random.Range(_spawnMin.x, _spawnMax.x);
                z = Random.Range(_spawnMin.y, _spawnMax.y);
            }
            
            return new Vector3(x, 100, z);
        }
        
        protected override void CleanUp()
        {
            base.CleanUp();
            _linkedCts?.Dispose();
            _waveCancellationTokenSource?.Dispose();
        }

        public void DetachListeners()
        {
            EventsAPI.Unregister<StartOfNewFloorEvent>(OnStartOfNewFloor);
            EventsAPI.Unregister<OnBossDiedEvent>(OnBossDiedEvent);

            if (!TimeManager.Instance) return;
            
            TimeManager.Instance.OnGamePaused -= PauseWaves;
            TimeManager.Instance.OnGameResumed -= ResumeWaves;

        }
    }
}
