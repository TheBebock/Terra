using NaughtyAttributes;
using UnityEngine;

namespace Terra.Components
{
    [RequireComponent(typeof(Light))]
    public class LightFlicker : MonoBehaviour
    {
        [SerializeField] private float minIntensity = 2f;
        [SerializeField] private float maxIntensity = 5f;
        [SerializeField] private float flickerSpeed = 0.1f;
 
        [SerializeField, ReadOnly]private Light _light;
        

        private void Update()
        {
            _light.intensity = Mathf.Lerp(minIntensity, maxIntensity, Mathf.PerlinNoise(Time.time * flickerSpeed, 0f));
        }


        private void OnValidate()
        {
            if (_light == null) _light = GetComponent<Light>();
        }
    }
}
