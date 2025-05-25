using UnityEngine;

namespace Terra.LootSystem.AirDrop
{
    public class LightFlicker : MonoBehaviour
    {
        [SerializeField] private float minIntensity = 2f;
        [SerializeField] private float maxIntensity = 5f;
        [SerializeField] private float flickerSpeed = 0.1f;
 
        private Light _lt;

        void Awake()
        {
            _lt = GetComponent<Light>();
        }

        private void Update()
        {
            _lt.intensity = Mathf.Lerp(minIntensity, maxIntensity, Mathf.PerlinNoise(Time.time * flickerSpeed, 0f));
        }
    }
}
