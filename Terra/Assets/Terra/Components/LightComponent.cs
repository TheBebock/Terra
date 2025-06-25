using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using NaughtyAttributes;

namespace Terra.Components
{
    [RequireComponent(typeof(Light))]
    public abstract class LightComponent : MonoBehaviour
    {
        [SerializeField, ReadOnly] protected Light _light;
        [SerializeField, ReadOnly] protected float _defaultIntensity;
        [SerializeField, ReadOnly] protected Color _defaultColor;
        
        public abstract void StartLightMode();
        protected abstract void OnUpdate();

        public abstract void StopLightMode();
        
        protected void ResetColor()
        {
            _light.color = _defaultColor;
        }

        protected void ResetIntensity()
        {
            _light.intensity = _defaultIntensity;
        }
        
        protected void Update()
        {
            OnUpdate();
        }
        
        public void DoFadeIntensity(float endValue, float duration)
        {
            StopLightMode();
            
            DOTween.To(
                () => _light.intensity,
                x => _light.intensity = x,
                endValue,
                duration
            ).WithCancellation(destroyCancellationToken);
        }
        protected virtual void OnValidate()
        {
            if (_light == null) _light = GetComponent<Light>();
            _defaultIntensity = _light.intensity;
            _defaultColor = _light.color;
        }
    }
}
