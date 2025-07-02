using System;
using Terra.Core.Generics;
using Terra.Interfaces;
using Terra.Utils;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Terra.PostProcess
{
    public class VolumesManager : PersistentMonoSingleton<VolumesManager>, IWithSetUp
    {
        [SerializeField] private float _currentGamma;
        [SerializeField] private float _currentBloom = 1;

        [SerializeField] private Vector2 _gammaRange = new(-1, 2);
        [SerializeField] private Vector2 _bloomRange = new(0f, 5f);

        [SerializeField] private Volume _globalVolume;
        [SerializeField] private Bloom _bloom;
        [SerializeField] private LiftGammaGain _gamma;

        public  Vector2 GammaRange => _gammaRange;
        
        public void SetUp()
        {
            if(Instance != this) return;
            SetGamma(GameSettings.DefaultGamma, false);
        }
        
        public void SetBloom(float value)
        {
            value = Mathf.Clamp(value, _bloomRange.x, _bloomRange.y);
            _currentBloom = value;
            _bloom.intensity.value = value;
        }

        public void SetGamma(float value, bool saveToSettings = true)
        {
            value = Mathf.Clamp(value, _gammaRange.x, _gammaRange.y);
            value = (float)Math.Round(value, 3);
            _currentGamma = value;
            _gamma.gamma.value = new Vector4(1, 1, 1, value);
            
            if(saveToSettings) GameSettings.DefaultGamma = value;
        }

#if UNITY_EDITOR
        
        private void OnValidate()
        {
            if (!_bloom) _bloom = _globalVolume.sharedProfile.TryGet(out Bloom bloom) ?  bloom : null;
            if (!_gamma) _gamma = _globalVolume.sharedProfile.TryGet(out LiftGammaGain gamma) ?  gamma : null;
            
            SetBloom(_currentBloom);
            SetGamma(_currentGamma, false);
        }
#endif
        



        public void TearDown()
        {
            //Noop
        }
    }
}
