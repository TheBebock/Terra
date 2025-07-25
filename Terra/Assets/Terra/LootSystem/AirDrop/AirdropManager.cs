﻿using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Terra.Core.Generics;
using Terra.Environment;
using Terra.EventsSystem;
using Terra.EventsSystem.Events;
using Terra.Interfaces;
using Terra.Itemization.Abstracts;
using Terra.Player;
using Terra.Utils;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Terra.LootSystem.AirDrop
{
    public class AirdropManager : MonoBehaviourSingleton<AirdropManager>, IWithSetUp, IAttachListeners
    {
        [FormerlySerializedAs("flarePrefab")] [SerializeField] private FlareLandingNotifier _flarePrefab;
        [FormerlySerializedAs("_pickupCratePrefab")] [FormerlySerializedAs("cratePrefab")] [SerializeField] private DamageablePickupCrate _pickupPickupCratePrefab;
        [FormerlySerializedAs("cratePrefab")] [SerializeField] private DamageableGunCrate _gunCratePrefab;
        [FormerlySerializedAs("dropContainer")] [SerializeField] private Transform _dropContainer;
        
        [FormerlySerializedAs("dropIntervalRange")] [SerializeField] private Vector2 _dropIntervalRange = new(60f, 90f);
        [FormerlySerializedAs("crateDelay")] [SerializeField] private float _crateDelay = 5f;
        [FormerlySerializedAs("crateHeightSpawnOffset")] [SerializeField] private float _crateHeightSpawnOffset = 10f;
        [FormerlySerializedAs("raycastLayerMask")] [SerializeField] private LayerMask _raycastLayerMask;
        [FormerlySerializedAs("raycastLayerMask")] [SerializeField] private LayerMask _objectsLayerMask;
        [FormerlySerializedAs("spawnMin")]
        [Header("Spawn Area")]
        [SerializeField] private Vector2 _spawnMin;
        [FormerlySerializedAs("spawnMax")] [SerializeField] private Vector2 _spawnMax;


        private float DifficultyModifier = 1;

        private Collider[] _colliders = new Collider[64];
        private LayerMask _groundLayer;
        private CancellationTokenSource _airdropCts;
        private CancellationTokenSource _linkedCts;
        protected override void Awake()
        {
            _groundLayer = LayerMask.NameToLayer("Ground");
        }

        public void AttachListeners()
        {
            EventsAPI.Register<GameDifficultyChangedEvent>(OnDifficultyChanged);
            EventsAPI.Register<OnBossDiedEvent>(OnBossDied);
        }

        public void SetUp()
        {
            SetDifficultyMultiplier();
            StartAirdrop();
        }

        private void OnDifficultyChanged(ref GameDifficultyChangedEvent gameDifficulty)
        {
            SetDifficultyMultiplier();
        }

        private void OnBossDied(ref OnBossDiedEvent ev)
        {
            StopAirDrop();
        }
        private void SetDifficultyMultiplier()
        {
            switch (GameSettings.DefaultDifficultyLevel)
            {
                case Enums.GameDifficulty.Cyberiada: DifficultyModifier = -20f; break;
                case Enums.GameDifficulty.Easy: DifficultyModifier = -10f; break;
                case Enums.GameDifficulty.Normal: DifficultyModifier = 0; break;
            }
        }

        private float GetModifiedSpawnDelay()
        {
            // Base interval values (in seconds) for airdrop spawn frequency
            float baseMin = _dropIntervalRange.x;
            float baseMax = _dropIntervalRange.y;

            // Get the player's current Luck value, clamped between 0 and 100 to avoid extreme cases
            int luck = Mathf.Clamp(PlayerStatsManager.Instance.PlayerStats.Luck, 0, 100); 

            // Compute a luck factor that scales the interval:
            // When luck = 0   → factor = 1.0 (no change)
            // When luck = 100 → factor = 0.5 (interval reduced by half)
            //
            // This means the higher the luck, the shorter the time between airdrops.
            // We're dividing by 200 to map 0–100 luck → 1.0–0.5 factor range.
            float luckFactor = Mathf.Lerp(1.0f, 0.75f, luck / 100f);

            // Scale the original time interval using the computed factor.
            // This results in a shorter interval for higher luck values.
            float modifiedMin = baseMin * luckFactor;
            float modifiedMax = baseMax * luckFactor;

            float finalMin = Mathf.Max(1, modifiedMin + DifficultyModifier);
            float finalMax = Mathf.Max(3, modifiedMax + DifficultyModifier);
            // Return a random value between the modified min and max delay
            return Random.Range(finalMin, finalMax);
        }

        private void StartAirdrop()
        {
            StopAirDrop();
            _airdropCts = new CancellationTokenSource();
            _linkedCts = CancellationTokenSource.CreateLinkedTokenSource(_airdropCts.Token, CancellationToken);
            _ = AirdropLoop(_linkedCts.Token);
        }
        private void StopAirDrop()
        {
            if (_airdropCts is { IsCancellationRequested: false })
            {
                _airdropCts?.Cancel();
            }
            
            _linkedCts?.Dispose();
            _airdropCts?.Dispose();
        }
        private async UniTaskVoid AirdropLoop(CancellationToken cancellationToken)
        {
            Debug.Log("Airdrop loop started.");
            try
            {
                while (true)
                {
                    float spawnDelay = GetModifiedSpawnDelay();

                    OnAirDropSetSpawnDelayEvent ev = new();
                    ev.time = spawnDelay;
                    EventsAPI.Invoke(ref ev);

                    await UniTask.WaitForSeconds(spawnDelay, cancellationToken: cancellationToken);

                    Debug.Log("Dropping flare...");

                    Vector3 dropPos = GetRandomPosition();
                    Debug.Log($"Flare drop position: {dropPos}");

                    _ = HandleDrop(dropPos);
                }
            }
            catch (OperationCanceledException)
            {
                Debug.Log("Airdrop loop canceled.");
            }
        }


        private Vector3 GetRandomPosition()
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
                        Debug.Log($"Found ground at: {hit.point}");
                        
                        var size = Physics.OverlapSphereNonAlloc(hit.point, 4f, _colliders, _groundLayer);
                        if( size <= 0 )
                        {
                            return hit.point;
                        }
                    }
                }
                atempts--;
                                
                x = Random.Range(_spawnMin.x, _spawnMax.x);
                z = Random.Range(_spawnMin.y, _spawnMax.y);
            }
            
            return new Vector3(x, 100, z);
        }

        private async UniTaskVoid HandleDrop(Vector3 groundPosition, ItemBase item = null)
        {
            Vector3 airPosition = groundPosition + Vector3.up * 20f;
            Debug.Log($"Instantiating flare at: {airPosition}");

            FlareLandingNotifier flare = Instantiate(_flarePrefab, airPosition, Quaternion.identity, _dropContainer);
            if (item != null)
            {
                flare.Init(Color.green);
            }
            bool landed = flare.HasLanded;
            if (!landed)
            {
                flare.onLanded += () =>
                {
                    Debug.Log("Flare landed event triggered.");
                    landed = true;
                };
            }
            

            await UniTask.WaitUntil( ()=> landed || CancellationToken.IsCancellationRequested, cancellationToken:CancellationToken);
            

            await UniTask.WaitForSeconds(_crateDelay, cancellationToken:CancellationToken);

            groundPosition.y += _crateHeightSpawnOffset;
            
            Debug.Log($"Instantiating crate at: {groundPosition}");

            if (item != null)
            {
                Instantiate(_gunCratePrefab, groundPosition, Quaternion.identity, _dropContainer).Init(item);
            }
            else
            {
                Instantiate(_pickupPickupCratePrefab, groundPosition, Quaternion.identity, _dropContainer);
            }
        }

        public void DropSelectedItem(ItemBase item)
        {
            Vector3 groundPosition = GetRandomPosition();
            
            _ = HandleDrop(groundPosition, item);
        }
        public void DetachListeners()
        {
            EventsAPI.Unregister<GameDifficultyChangedEvent>(OnDifficultyChanged);
            EventsAPI.Unregister<OnBossDiedEvent>(OnBossDied);
        }
        
        public void TearDown()
        {
            _linkedCts?.Dispose();
            _airdropCts?.Dispose();
        }

    }
}
