using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

namespace Terra.Components
{
    [RequireComponent(typeof(Light))]
    public class LightFlicker : MonoBehaviour
    {
        [SerializeField] private bool _isFlickering;
        [FormerlySerializedAs("minIntensity")] [SerializeField] private float _minIntensity = 2f;
        [FormerlySerializedAs("maxIntensity")] [SerializeField] private float _maxIntensity = 5f;
        [FormerlySerializedAs("flickerSpeed")] [SerializeField] private float _flickerSpeed = 0.1f;
 
        [SerializeField, ReadOnly] private Light _light;
        [SerializeField, ReadOnly] private float _defaultIntensity;
       

        private void Update()
        {
            if(_isFlickering)
                _light.intensity = Mathf.Lerp(_minIntensity, _maxIntensity, Mathf.PerlinNoise(Time.time * _flickerSpeed, 0f));
        }

        public void StopFlickering()
        {
            _isFlickering = false;
            _light.intensity = _defaultIntensity;
        }

        public void StartFlickering()
        {
            _isFlickering = true;
        }

        private void OnValidate()
        {
            if (_light == null) _light = GetComponent<Light>();
            _defaultIntensity = _light.intensity;
        }
    }
}
