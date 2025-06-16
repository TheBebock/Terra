using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using Terra.Core.Generics;
using Terra.Interfaces;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Terra.LootSystem.AirDrop
{
    public class AirdropManager : MonoBehaviourSingleton<AirdropManager>, IWithSetUp
    {
        [SerializeField] private FlareLandingNotifier flarePrefab;
        [SerializeField] private GameObject cratePrefab;
        [SerializeField] private Transform dropContainer;
        
        [SerializeField] private Vector2 _dropIntervalRange = new(60f, 90f);
        [SerializeField] private float _crateDelay = 5f;
        [SerializeField] private float _crateHeightSpawnOffset = 10f;
        [SerializeField] private LayerMask _raycastLayerMask;
        [Header("Spawn Area")]
        [SerializeField] private Vector2 _spawnMin;
        [SerializeField] private Vector2 _spawnMax;
        
        private LayerMask _groundLayer;

        protected override void Awake()
        {
            _groundLayer = LayerMask.NameToLayer("Ground");
        }

        public void SetUp()
        {
            _ = AirdropLoop();
        }
        
        private async UniTaskVoid AirdropLoop()
        {
            Debug.Log("Airdrop loop started.");
            while (true)
            {
                float spawnDelay = Random.Range(_dropIntervalRange.x, _dropIntervalRange.y); ;
                await UniTask.WaitForSeconds(spawnDelay, cancellationToken:CancellationToken);

                Debug.Log("Dropping flare...");

                Vector3 dropPos = GetRandomPosition();
                Debug.Log($"Flare drop position: {dropPos}");

                _ = HandleDrop(dropPos);
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
                        return hit.point;
                    }
                }
                atempts--;
                                
                x = Random.Range(_spawnMin.x, _spawnMax.x);
                z = Random.Range(_spawnMin.y, _spawnMax.y);
            }
            
            return new Vector3(x, 100, z);
        }

        private async UniTaskVoid HandleDrop(Vector3 groundPosition)
        {
            Vector3 airPosition = groundPosition + Vector3.up * 100f;
            Debug.Log($"Instantiating flare at: {airPosition}");

            FlareLandingNotifier flare = Instantiate(flarePrefab, airPosition, Quaternion.identity, dropContainer);
            
            bool landed = flare.HasLanded;
            if (!landed)
            {
                flare.OnLanded += () =>
                {
                    Debug.Log("Flare landed event triggered.");
                    landed = true;
                };
            }
            

            await UniTask.WaitUntil( ()=> landed, cancellationToken:CancellationToken);
            

            await UniTask.WaitForSeconds(_crateDelay, cancellationToken:CancellationToken);

            groundPosition.y += _crateHeightSpawnOffset;
            
            Debug.Log($"Instantiating crate at: {groundPosition}");
            
            Instantiate(cratePrefab, groundPosition, Quaternion.identity, dropContainer);
        }
        
        
        public void TearDown()
        {
            
        }
    }
}
