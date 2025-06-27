using Cysharp.Threading.Tasks;
using Terra.Core.Generics;
using Terra.Interfaces;
using Terra.Player;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Terra.LootSystem.AirDrop
{
    public class AirdropManager : MonoBehaviourSingleton<AirdropManager>, IWithSetUp
    {
        [FormerlySerializedAs("flarePrefab")] [SerializeField] private FlareLandingNotifier _flarePrefab;
        [FormerlySerializedAs("cratePrefab")] [SerializeField] private GameObject _cratePrefab;
        [FormerlySerializedAs("dropContainer")] [SerializeField] private Transform _dropContainer;
        
        [FormerlySerializedAs("dropIntervalRange")] [SerializeField] private Vector2 _dropIntervalRange = new(60f, 90f);
        [FormerlySerializedAs("crateDelay")] [SerializeField] private float _crateDelay = 5f;
        [FormerlySerializedAs("crateHeightSpawnOffset")] [SerializeField] private float _crateHeightSpawnOffset = 10f;
        [FormerlySerializedAs("raycastLayerMask")] [SerializeField] private LayerMask _raycastLayerMask;
        [FormerlySerializedAs("spawnMin")]
        [Header("Spawn Area")]
        [SerializeField] private Vector2 _spawnMin;
        [FormerlySerializedAs("spawnMax")] [SerializeField] private Vector2 _spawnMax;
        
        private LayerMask _groundLayer;

        protected override void Awake()
        {
            _groundLayer = LayerMask.NameToLayer("Ground");
        }

        public void SetUp()
        {
            _ = AirdropLoop();
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

            // Return a random value between the modified min and max delay
            return Random.Range(modifiedMin, modifiedMax);
        }


        private async UniTaskVoid AirdropLoop()
        {
            Debug.Log("Airdrop loop started.");
            while (true)
            {
                float spawnDelay = GetModifiedSpawnDelay();
                await UniTask.WaitForSeconds(spawnDelay, cancellationToken:CancellationToken);

                Debug.Log("Dropping flare...");

                Vector3 dropPos = GetRandomPosition();
                Debug.Log($"Flare drop position: {dropPos}");

                _ = HandleDrop(dropPos);
            }
            // ReSharper disable once FunctionNeverReturns
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

            FlareLandingNotifier flare = Instantiate(_flarePrefab, airPosition, Quaternion.identity, _dropContainer);
            
            bool landed = flare.HasLanded;
            if (!landed)
            {
                flare.onLanded += () =>
                {
                    Debug.Log("Flare landed event triggered.");
                    landed = true;
                };
            }
            

            await UniTask.WaitUntil( ()=> landed, cancellationToken:CancellationToken);
            

            await UniTask.WaitForSeconds(_crateDelay, cancellationToken:CancellationToken);

            groundPosition.y += _crateHeightSpawnOffset;
            
            Debug.Log($"Instantiating crate at: {groundPosition}");
            
            Instantiate(_cratePrefab, groundPosition, Quaternion.identity, _dropContainer);
        }
        
        
        public void TearDown()
        {
            
        }
    }
}
