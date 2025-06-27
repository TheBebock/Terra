using Terra.Core.Generics;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Terra.PostProcess
{
    public class VolumesManager : PersistentMonoSingleton<VolumesManager>
    {
        [SerializeField] private float _currentGamma;
        [SerializeField] private float _currentBloom = 1;

        [SerializeField] private Vector2 _gammaRange = new(-1, 2);
        [SerializeField] private Vector2 _bloomRange = new(0f, 5f);

        [SerializeField] private Volume _globalVolume;
        [SerializeField] private Bloom _bloom;
        [SerializeField] private LiftGammaGain _gamma;

        public void SetBloom(float value)
        {
            value = Mathf.Clamp(value, _bloomRange.x, _bloomRange.y);
            _bloom.intensity.value = value;
        }

        public void SetGamma(float value)
        {
            value = Mathf.Clamp(value, _gammaRange.x, _gammaRange.y);
            _gamma.gamma.value = new Vector4(value, value, value, value);
        }

        private void OnValidate()
        {
            if (!_bloom) _bloom = _globalVolume.sharedProfile.TryGet(out Bloom bloom) ?  bloom : null;
            if (!_gamma) _gamma = _globalVolume.sharedProfile.TryGet(out LiftGammaGain gamma) ?  gamma : null;
            
            SetBloom(_currentBloom);
            SetGamma(_currentGamma);
        }
    }
}
