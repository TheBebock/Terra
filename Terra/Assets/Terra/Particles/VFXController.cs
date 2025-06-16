using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using NaughtyAttributes;
using Terra.Components;
using Terra.Core.Generics;
using Terra.Extensions;
using UnityEngine;

namespace Terra.Particles
{
    [RequireComponent(typeof(Entity))]
    public class VFXController : MonoBehaviour
    {
        [Foldout("Particles")]  public ParticleComponent onSpawnParticle;
        [Foldout("Particles")]  public ParticleComponent onHealParticle;
        [Foldout("Particles")]  public ParticleComponent onHitParticle;
        [Foldout("Particles")]  public ParticleComponent onDeathParticle;
        [Foldout("References")][SerializeField] private Entity _entity;
        [Foldout("References")][SerializeField] private SpriteRenderer _entityModel;
       
        [Foldout("Debug")][SerializeField] private Material _modelMaterial;
        [Foldout("Debug")][SerializeField] private List<ParticleComponent> _activeParticles;
        [Foldout("Debug")][SerializeField] private Color _defaultColor;
        
        public SpriteRenderer EntityModel => _entityModel;
        private Vector3 Position => _entity.transform.position;
        private Quaternion Rotation => _entityModel.transform.rotation;
        
        private CancellationTokenSource _blinkCts;
        private void Start()
        {
            _modelMaterial = _entityModel.material;
            _defaultColor = _modelMaterial.color;
            ParticleComponent.OnParticleDestroyed += OnParticleDestroyed;
        }

        private void OnParticleDestroyed(ParticleComponent particle)
        {
            _activeParticles.RemoveElement(particle);
        }
        
        public void SetModelTransparency(float value)
        {
            value = Mathf.Clamp(value, 0, 1);
            Color color = _modelMaterial.color;
            color.a = value;
            _modelMaterial.color = color;
        }
        public void DoFadeModel(float endValue, float duration, AnimationCurve curve = null)
        {
            if (curve == null)
            {
                curve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
            }
            
            endValue = Mathf.Clamp(endValue, 0, 1);
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

        public void PlayParticleOnEntity(ParticleComponent particle) => 
            SpawnAndAttachParticleToEntity(_entity, particle); 
        
        public static void SpawnAndAttachParticleToEntity(Entity entity, ParticleComponent particleSystem, 
            Vector3 positionOffset = default, Quaternion rotation = default,
            float scaleModifier = 1.0f, float destroyDuration = 0f)
        {
            if (!particleSystem)
            {
                Debug.LogError($"Tried to spawn particle system on {entity.gameObject.name}, " +
                               $"but particle system is null");
                return;
            }
            
            if (entity.VFXController._activeParticles.TryFind(p=> p == particleSystem,
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
            entity.VFXController._activeParticles.Add(particle);
        }
        public static void SpawnParticleInWorld(ParticleComponent particleSystem, Vector3 position, Quaternion rotation, 
            float scaleModifier = 1.0f, float destroyDuration = 0f)
        {
            if (!particleSystem)
            {
                Debug.LogError($"Tried to spawn particle system on coordinates {position}, but it's null");
                return;
            }
            ParticleComponent particle = Instantiate(particleSystem, position, rotation);
            particle.transform.localScale *= scaleModifier;
            
            particle.Initialize(destroyDuration);
        }

        private void OnValidate()
        {
            if (_entity != null)
            {
                if(!_entityModel) _entityModel = _entity.GetComponentInChildren<SpriteRenderer>();
            }
        }
    }
}
