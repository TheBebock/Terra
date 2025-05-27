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

        [SerializeField] private float _distanceToEdge = 0.5f;
        [SerializeField] private Vector2 _dropIntervalRange = new(60f, 90f);
        [SerializeField] private float _crateDelay = 5f;
        [SerializeField] private float _crateHeightSpawnOffset = 10f;
        [Foldout("Debug"), ReadOnly][SerializeField]private Vector3 spawnAreaCenter;
        [Foldout("Debug"), ReadOnly][SerializeField] private Vector3 spawnAreaSize;

        private LayerMask _groundLayer;
        

        protected override void Awake()
        {
            _groundLayer = LayerMask.NameToLayer("Ground");
            CalculateGroundBounds();
        }

        public void SetUp()
        {
            _ = AirdropLoop();
        }
        
        private void CalculateGroundBounds()
        {
            Collider[] groundColliders = FindObjectsOfType<Collider>();

            Bounds combined = new Bounds();
            bool hasAny = false;

            foreach (var col in groundColliders)
            {
                if (col.gameObject.layer != LayerMask.NameToLayer("Ground"))
                    continue;

                if (!hasAny)
                {
                    combined = new Bounds(col.bounds.center, col.bounds.size);
                    hasAny = true;
                }
                else
                {
                    combined.Encapsulate(col.bounds);
                }
            }

            if (!hasAny)
            {
                Debug.LogWarning("Nie znaleziono colliderów na warstwie 'Ground'");
                return;
            }

            spawnAreaCenter = combined.center;
            spawnAreaSize = new Vector3(combined.size.x - _distanceToEdge, 0f, combined.size.z - _distanceToEdge);

            Debug.Log($"Ground bounds: Center={spawnAreaCenter}, Size={spawnAreaSize}");
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
            float halfX = spawnAreaSize.x / 2f;
            float halfZ = spawnAreaSize.z / 2f;

            float x = Random.Range(spawnAreaCenter.x - halfX, spawnAreaCenter.x + halfX);
            float z = Random.Range(spawnAreaCenter.z - halfZ, spawnAreaCenter.z + halfZ);

            Vector3 rayOrigin = new Vector3(x, spawnAreaCenter.y + 20f, z);
            if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, 200f, _groundLayer))
            {
                Debug.Log($"Found ground at: {hit.point}");
                return hit.point;
            }

            Debug.LogWarning("Raycast failed! Returning fallback spawn position.");
            return new Vector3(x, spawnAreaCenter.y, z);
        }

        private async UniTaskVoid HandleDrop(Vector3 groundPosition)
        {
            Vector3 airPosition = groundPosition + Vector3.up * 100f;
            Debug.Log($"Instantiating flare at: {airPosition}");

            FlareLandingNotifier flare = Instantiate(flarePrefab, airPosition, Quaternion.identity, dropContainer);


            if (flare == null)
            {
                Debug.LogError("Flare prefab nie ma komponentu FlareLandingNotifier!");
                
            }

            bool landed = flare.HasLanded;
            if (!landed)
            {
                flare.OnLanded += () =>
                {
                    Debug.Log("Flare landed event triggered.");
                    landed = true;
                };
            }

            Debug.Log("Czekam aż flara wyląduje...");

            await UniTask.WaitUntil( ()=> landed, cancellationToken:CancellationToken);
              

            Debug.Log("Flara wylądowała. Czekam na skrzynkę...");

            await UniTask.WaitForSeconds(_crateDelay, cancellationToken:CancellationToken);

            groundPosition.y += _crateHeightSpawnOffset;
            
            Debug.Log($"Instantiating crate at: {groundPosition}");
            
            Instantiate(cratePrefab, groundPosition, Quaternion.identity, dropContainer);
        }

        private void OnDrawGizmosSelected()
        {
            if (spawnAreaSize != Vector3.zero)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireCube(spawnAreaCenter, spawnAreaSize);
            }
        }
        
        public void TearDown()
        {
            
        }
    }
}
