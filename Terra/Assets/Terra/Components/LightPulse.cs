using NaughtyAttributes;
using UnityEngine;

namespace Terra.Components
{
    public class LightPulse : LightComponent
    {
        [SerializeField] private bool _isPulsating;
        [SerializeField] private bool _pulseToAnotherColor;
        [ShowIf(nameof(_pulseToAnotherColor))][SerializeField]
        private Color _pulseColor = new(1f, 1f, 1f, 1f);
        [SerializeField] private float _minIntensity = 0.1f;
        [SerializeField] private float _maxIntensity = 5f;
        [SerializeField] private float _pulseSpeed = 0.1f;


        public override void StartLightMode()
        {
            _light.color = _pulseColor;
            _isPulsating = true;
        }

        protected override void OnUpdate()
        {
            if (!_isPulsating) return;
            
            float t = Mathf.PingPong(Time.time * _pulseSpeed, 1f);
            _light.intensity = Mathf.Lerp(_minIntensity, _maxIntensity, t);
            if (_pulseToAnotherColor)
            {
                _light.color = Color.Lerp(_defaultColor, _pulseColor, t);
            }
        }

        public override void StopLightMode()
        {
            _isPulsating = false;
            ResetColor();
            ResetIntensity();
        }
    }
}
