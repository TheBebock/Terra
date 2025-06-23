using NaughtyAttributes;
using UnityEngine;

namespace Terra.Components
{
    [RequireComponent(typeof(Light))]
    public class LightPulse : MonoBehaviour
    {
        [SerializeField] private bool _isPulsating;
        [SerializeField] private Color _pulseColor = new(1f, 1f, 1f, 1f);
        [SerializeField] private float _minIntensity = 0.1f;
        [SerializeField] private float _maxIntensity = 5f;
        [SerializeField] private float _pulseSpeed = 0.1f;
 
        [SerializeField, ReadOnly] private Light _light;
        [SerializeField, ReadOnly] private float _defaultIntensity;
        [SerializeField, ReadOnly] private Color _defaultColor;
       

        private void Update()
        {
            if (!_isPulsating) return;
            
            float t = Mathf.PingPong(Time.time * _pulseSpeed, 1f);
            _light.intensity = Mathf.Lerp(_minIntensity, _maxIntensity, t);
        }
        
        private void ResetColor()
        {
            _light.color = _defaultColor;
        }
        public void StopPulsating()
        {
            _isPulsating = false;
            ResetColor();
            _light.intensity = _defaultIntensity;
        }

        public void StartPulsating()
        {
            _light.color = _pulseColor;
            _isPulsating = true;
        }

        private void OnValidate()
        {
            if (_light == null) _light = GetComponent<Light>();
            _defaultIntensity = _light.intensity;
            _defaultColor = _light.color;
        }
    }
}
