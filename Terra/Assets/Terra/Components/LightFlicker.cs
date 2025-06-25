using UnityEngine;
using UnityEngine.Serialization;

namespace Terra.Components
{
    public class LightFlicker : LightComponent
    {
        [SerializeField] private bool _isFlickering;
        [FormerlySerializedAs("minIntensity")] [SerializeField] private float _minIntensity = 2f;
        [FormerlySerializedAs("maxIntensity")] [SerializeField] private float _maxIntensity = 5f;
        [FormerlySerializedAs("flickerSpeed")] [SerializeField] private float _flickerSpeed = 0.1f;

        public override void StartLightMode()
        {
            _isFlickering = true;
        }

        protected override void OnUpdate()
        {
            if(_isFlickering)
                _light.intensity = Mathf.Lerp(_minIntensity, _maxIntensity, Mathf.PerlinNoise(Time.time * _flickerSpeed, 0f));
        }

        public override void StopLightMode()
        {
            _isFlickering = false;
            ResetIntensity();
        }
    }
}
