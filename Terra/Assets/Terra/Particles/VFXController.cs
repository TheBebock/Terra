using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using JetBrains.Annotations;
using NaughtyAttributes;
using Terra.Components;
using Terra.Core.Generics;
using Terra.Extensions;
using UnityEngine;

namespace Terra.Particles
{
    public class VFXController : MonoBehaviour
    {
        private static readonly int EmissiveIntensity = Shader.PropertyToID("_EmissiveIntensity");
        private static readonly int EmissiveMask = Shader.PropertyToID("_EmissiveMask");
        [Foldout("Particles")]  public ParticleComponent onSpawnParticle;
        [Foldout("Particles")]  public ParticleComponent onHealParticle;
        [Foldout("Particles")]  public ParticleComponent onHitParticle;
        [Foldout("Particles")]  public ParticleComponent onDeathParticle;
        [Foldout("References")][SerializeField] private Transform _container;
        [Foldout("References")][SerializeField] private SpriteRenderer _model;
       
        [Foldout("Debug")][SerializeField] private Material _modelMaterial;
        [Foldout("Debug")][SerializeField] private List<ParticleComponent> _activeParticles;
        [Foldout("Debug")][SerializeField] private Color _defaultColor;
        
        public SpriteRenderer Model => _model;
        
        private CancellationTokenSource _blinkCts;
        private CancellationTokenSource _fadeCts;
        private void Start()
        {
            _modelMaterial = _model.material;
            _defaultColor = _modelMaterial.color;
            ParticleComponent.OnParticleDestroyed += OnParticleDestroyed;
        }

        private void OnParticleDestroyed(ParticleComponent particle)
        {
            _activeParticles.RemoveElement(particle);
        }

        public void SetModelSprite(Sprite sprite)
        {
            _model.sprite = sprite;
        }

        public void SetModelMaterial(Material material)
        {
            _modelMaterial = material;
        }
        public void SetModelTransparency(float value)
        {
            value = Mathf.Clamp(value, 0, 1);
            Color color = _modelMaterial.color;
            color.a = value;
            _modelMaterial.color = color;
        }

        [UsedImplicitly]
        public void SetMaterialEmissiveMap(Texture map)
        {
            _modelMaterial.SetTexture(EmissiveMask, map);
        }
        public void DoFadeModel(float endValue, float duration, AnimationCurve curve = null)
        {
            if (curve == null || curve.keys.Length == 0)
            {
                curve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
            }
            endValue = Mathf.Clamp(endValue, 0, 1);
            _fadeCts?.Cancel();
            _fadeCts?.Dispose();
            _fadeCts = new CancellationTokenSource();

            float emissiveIntensityClamped = Mathf.Clamp(endValue, 0, _modelMaterial.GetFloat(EmissiveIntensity));
            
            DOTween.To(
                () => _modelMaterial.GetFloat(EmissiveIntensity),
                x => _modelMaterial.SetFloat(EmissiveIntensity, x),
                emissiveIntensityClamped,
                duration/2
            ).SetEase(curve)
            .WithCancellation(_fadeCts.Token);
            
            _modelMaterial.DOFade(endValue, duration).SetEase(curve);
        }

        public void BlinkModelsColor(Color color, float fadeInDuration, float pauseDuration,
            float fadeOutDuration)
        {
            _blinkCts?.Cancel();
            _blinkCts?.Dispose();
            _blinkCts = new CancellationTokenSource();
            BlinkModelsColorAsync(color,fadeInDuration, pauseDuration, fadeOutDuration, _blinkCts.Token).Forget();
        }
        private async UniTaskVoid BlinkModelsColorAsync(Color color, float fadeInDuration,float pauseDuration,
            float fadeOutDuration, CancellationToken token)
        {
            await _modelMaterial.DOColor(color, fadeInDuration).WithCancellation(cancellationToken:token);
            await UniTask.WaitForSeconds(pauseDuration, cancellationToken:token);
            await _modelMaterial.DOColor(_defaultColor, fadeOutDuration).WithCancellation(cancellationToken:token);
        }
        
        public static void SpawnAndAttachParticleToEntity(Entity entity, ParticleComponent particleSystem, 
            Vector3 positionOffset = default, Quaternion rotation = default,
            float scaleModifier = 1.0f, float destroyDuration = 0f)
        {
            if (!particleSystem)
            {
                Debug.LogWarning($"Tried to spawn particle system on {entity.gameObject.name}, " +
                               $"but particle system is null");
                return;
            }
            
            if (entity.VFXcontroller._activeParticles.TryFind(p=> p == particleSystem,
                    out ParticleComponent foundParticle))
            {
                 if(destroyDuration > 0f) foundParticle.ResetTimer(destroyDuration);
                 else foundParticle.ResetTimer();
                 return;
            }
            
            ParticleComponent particle = Instantiate(particleSystem, entity.transform);
            particle.transform.localPosition = positionOffset;
            particle.transform.rotation = rotation;
            particle.transform.localScale *= scaleModifier;
            
            particle.Initialize(destroyDuration);
            entity.VFXcontroller._activeParticles.Add(particle);
        }
        public static void SpawnParticleInWorld(ParticleComponent particleSystem, Vector3 position, Quaternion rotation, 
            float scaleModifier = 1.0f, float destroyDuration = 0f)
        {
            if (!particleSystem)
            {
                Debug.LogWarning($"Tried to spawn particle system on coordinates {position}, but it's null");
                return;
            }
            ParticleComponent particle = Instantiate(particleSystem, position, rotation);
            particle.transform.localScale *= scaleModifier;
            
            particle.Initialize(destroyDuration);
        }
        
        private void OnValidate()
        {
            if (!_container)
            {
                _container = GetComponent<Transform>();
            }
            if (_container)
            {
                 _model = _container.GetComponentInChildren<SpriteRenderer>();
            }
        }
    }
}
