using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using JetBrains.Annotations;
using NaughtyAttributes;
using Terra.Components;
using Terra.Core.Generics;
using Terra.Extensions;
using Terra.Interfaces;
using UnityEngine;

namespace Terra.Particles
{
    public class VFXController : InGameMonobehaviour, IAttachListeners
    {
        private static readonly int EmissiveIntensity = Shader.PropertyToID("_EmissiveIntensity");
        private static readonly int EmissiveMask = Shader.PropertyToID("_EmissiveMask");
        private static readonly int ColorID = Shader.PropertyToID("_Color");

        [Foldout("Particles")][SerializeField] public ParticleComponentData onSpawnParticle;
        [Foldout("Particles")][SerializeField] public ParticleComponentData onHealParticle;
        [Foldout("Particles")][SerializeField] public ParticleComponentData onHitParticle;
        [Foldout("Particles")][SerializeField] public ParticleComponentData onDeathParticle;

        [Foldout("References")][SerializeField] private Transform _container;
        [Foldout("References")][SerializeField] private Transform _particlesAnchor;
        [Foldout("References")][SerializeField] private SpriteRenderer _model;
       
        [Foldout("Debug")][SerializeField] private Material _modelMaterial;
        [Foldout("Debug")][SerializeField] private List<ParticleComponent> _activeParticles;
        [Foldout("Debug")][SerializeField] private Color _defaultColor;
        
        private MaterialPropertyBlock _materialPropertyBlock;
        public SpriteRenderer Model => _model;
        public Transform ParticlesAnchor => _particlesAnchor;
        private CancellationTokenSource _blinkCts;
        private CancellationTokenSource _fadeCts;

        private void Awake()
        {
            _modelMaterial = _model.material;

            _materialPropertyBlock = new MaterialPropertyBlock();
            
            _model.GetPropertyBlock(_materialPropertyBlock);
            _defaultColor = _modelMaterial.color;
        }
        
        public void AttachListeners()
        {
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
            _model.material = material;
        }
        public void SetModelTransparency(float value)
        {
            value = Mathf.Clamp01(value);
            
            Color color = _modelMaterial.GetColor(ColorID);

            color.a = value;

            _model.GetPropertyBlock(_materialPropertyBlock);
            _materialPropertyBlock.SetColor(ColorID, color);
            _model.SetPropertyBlock(_materialPropertyBlock);
        }

        public Transform GetParticlesContainer(bool useAnchor = false)
        {
            if (useAnchor && _particlesAnchor != null)
            {
                return _particlesAnchor;
            }

            return _container;
        }
        [UsedImplicitly]
        public void SetMaterialEmissiveMap(Texture map)
        {
            _model.GetPropertyBlock(_materialPropertyBlock);
            _materialPropertyBlock.SetTexture(EmissiveMask, map);
            _model.SetPropertyBlock(_materialPropertyBlock);
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
            
          _ = DoFadeAsync(endValue, emissiveIntensityClamped, duration, curve);
            
        }

        private async UniTaskVoid DoFadeAsync(float fadeEndValue, float emissiveEndValue, float duration, AnimationCurve curve = null)
        {
            DOTween.To(
                    () => _modelMaterial.GetFloat(EmissiveIntensity),
                    x => _modelMaterial.SetFloat(EmissiveIntensity, x),
                    emissiveEndValue,
                    duration/2
                ).SetEase(curve)
                .WithCancellation(_fadeCts.Token);
            
            _modelMaterial.DOFade(fadeEndValue, duration).SetEase(curve);

            //Disable shadows
            await UniTask.WaitForSeconds((duration*0.6f), cancellationToken: CancellationToken);
            if (_modelMaterial.GetColor(ColorID).a < 0.8f)
            {
                _model.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            }
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
            float scaleModifier = 1.0f, float destroyDuration = 0f, bool useParticlesAnchor = false)
        {
            if (!particleSystem)
            {
                Debug.LogWarning($"Tried to spawn particle system on {entity.gameObject.name}, " +
                               $"but particle system is null");
                return;
            }


            foreach (var p in entity.VFXcontroller._activeParticles)
            {
                if (p.Identifier.Equals(particleSystem.Identifier, StringComparison.OrdinalIgnoreCase))
                {
                    // If the particle is looped, reset its timer
                    if (p.DestroyDuration > 0 && p.IsLooped)
                    {
                        p.ResetTimer();
                        return;
                    }
                    // If particle is not playing and isn't destroyed on end, reset it timer
                    if (!p.IsPlaying && !p.IsDestroyable)
                    {
                        p.ResetTimer();
                        return;
                    }
                }
            }

            Transform particleContainer = entity.VFXcontroller.GetParticlesContainer(useParticlesAnchor);
            ParticleComponent particle = Instantiate(particleSystem, particleContainer);
            particle.transform.localPosition = positionOffset;
            particle.transform.rotation = rotation;
            particle.transform.localScale *= scaleModifier;
            
            particle.Initialize(destroyDuration);
            entity.VFXcontroller._activeParticles.Add(particle);
        }

        public static void SpawnAndAttachParticleToEntity(Entity entity, ParticleComponentData particleComponentData)
        {
            SpawnAndAttachParticleToEntity(entity, 
                particleComponentData.particleComponent, 
                particleComponentData.offset,
                particleComponentData.rotation,
                particleComponentData.scaleModifier != default ? particleComponentData.scaleModifier : 1.0f,
                particleComponentData.destroyDuration);
        }

        public static void SpawnAndAttachParticleToEntityOnAnchor(Entity entity, ParticleComponent particleComponent, float destroyDuration =0f)
        {
            SpawnAndAttachParticleToEntity(entity,
                particleComponent,
                destroyDuration: destroyDuration,
                useParticlesAnchor: true);
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

        public static void SpawnParticleInWorld(ParticleComponentData particleComponentData)
        {
            SpawnParticleInWorld(particleComponentData.particleComponent,
                position: particleComponentData.offset,
                rotation: particleComponentData.rotation,
                scaleModifier: particleComponentData.scaleModifier != default ? particleComponentData.scaleModifier : 1.0f,
                destroyDuration: particleComponentData.destroyDuration);
        }

        private void RemoveParticle(ParticleComponent particles)
        {
            foreach (var p in _activeParticles)
            {
                if (p.Identifier.Equals(particles.Identifier, StringComparison.OrdinalIgnoreCase))
                {
                    p.KillParticles();
                    return;
                }
            }
            
            Debug.LogWarning($"{this}: Tried to remove particle {particles.Identifier}, but it doesn't exist.");
        }
        public static void RemoveParticleFromEntity(Entity entity, ParticleComponent particleComponent)
        {
            if (!entity)
            {
                Debug.LogError($"Entity to remove from was null");
                return;
            }

            if (!particleComponent)
            {
                Debug.LogError($"Particle to remove was null");
                return;
            }
            entity.VFXcontroller.RemoveParticle(particleComponent);
        }
        


        public void DetachListeners()
        {
            ParticleComponent.OnParticleDestroyed -= OnParticleDestroyed;
        }
        
#if UNITY_EDITOR

        protected override void OnValidate()
        {
            base.OnValidate();
            
            if (!_container)
            {
                _container = GetComponent<Transform>();
            }
            if (_container)
            {
                _model = _container.GetComponentInChildren<SpriteRenderer>();
            }
        }
#endif
    }
}
