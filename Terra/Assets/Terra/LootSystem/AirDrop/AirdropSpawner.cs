using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Terra.LootSystem.AirDrop
{
    public class AirdropSpawner : MonoBehaviour
    {
        public GameObject flarePrefab;
        public GameObject cratePrefab;
        [SerializeField] private Transform dropContainer;

        public float dropInterval = 60f;
        public float crateDelay = 5f;

        [HideInInspector]
        public Vector3 spawnAreaCenter;
        [HideInInspector]
        public Vector3 spawnAreaSize;

        private LayerMask _groundLayer;
        private float _distanceToEdge = 0.5f;

        void Start()
        {
            _groundLayer = LayerMask.GetMask("Ground");
            CalculateGroundBounds();
            StartCoroutine(AirdropLoop());
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

        private IEnumerator AirdropLoop()
        {
            Debug.Log("Airdrop loop started.");
            while (true)
            {
                yield return new WaitForSeconds(dropInterval);

                Debug.Log("Dropping flare...");

                Vector3 dropPos = GetRandomPosition();
                Debug.Log($"Flare drop position: {dropPos}");

                StartCoroutine(HandleDrop(dropPos));
            }
        }

        private Vector3 GetRandomPosition()
        {
            float halfX = spawnAreaSize.x / 2f;
            float halfZ = spawnAreaSize.z / 2f;

            float x = Random.Range(spawnAreaCenter.x - halfX, spawnAreaCenter.x + halfX);
            float z = Random.Range(spawnAreaCenter.z - halfZ, spawnAreaCenter.z + halfZ);

            Vector3 rayOrigin = new Vector3(x, spawnAreaCenter.y + 100f, z);
            if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, 200f, _groundLayer))
            {
                Debug.Log($"Found ground at: {hit.point}");
                return hit.point;
            }

            Debug.LogWarning("Raycast failed! Returning fallback spawn position.");
            return new Vector3(x, spawnAreaCenter.y, z);
        }

        private IEnumerator HandleDrop(Vector3 groundPosition)
        {
            Vector3 airPosition = groundPosition + Vector3.up * 100f;
            Debug.Log($"Instantiating flare at: {airPosition}");

            GameObject flare = Instantiate(flarePrefab, airPosition, Quaternion.identity, dropContainer);
            FlareLandingNotifier notifier = flare.GetComponent<FlareLandingNotifier>();

            if (notifier == null)
            {
                Debug.LogError("Flare prefab nie ma komponentu FlareLandingNotifier!");
                yield break;
            }

            bool landed = notifier.HasLanded;
            if (!landed)
            {
                notifier.OnLanded += () =>
                {
                    Debug.Log("Flare landed event triggered.");
                    landed = true;
                };
            }

            Debug.Log("Czekam aż flara wyląduje...");

            while (!landed)
                yield return null;

            Debug.Log("Flara wylądowała. Czekam na skrzynkę...");

            yield return new WaitForSeconds(crateDelay);

            Debug.Log($"Instantiating crate at: {groundPosition + Vector3.up * 1f}");
            Instantiate(cratePrefab, groundPosition + Vector3.up * 1f, Quaternion.identity, dropContainer);
        }

        private void OnDrawGizmosSelected()
        {
            if (spawnAreaSize != Vector3.zero)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireCube(spawnAreaCenter, spawnAreaSize);
            }
        }
    }
}
