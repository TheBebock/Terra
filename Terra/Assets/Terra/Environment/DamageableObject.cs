using DG.Tweening;
using NaughtyAttributes;
using Terra.AI.EnemyStates;
using Terra.Combat;
using Terra.Core.Generics;
using Terra.Core.ModifiableValue;
using Terra.EffectsSystem;
using Terra.Interfaces;
using Terra.Managers;
using Terra.Particles;
using UnityEngine;
using UnityEngine.Serialization;

namespace Terra.Environment
{

    /// <summary>
    /// Represents object in game world that can be damaged
    /// </summary>
    public class DamageableObject : Entity, IDamageable, IAttachListeners
    {
        [SerializeField] private ModifiableValue _maxHealth;
        [SerializeField] private float _deathFadeDuration = 2.5f;
        [SerializeField] private AnimationCurve _deathFadeCurve;
        
        [Foldout("References")][SerializeField] protected Animator _propAnimator;
        [Foldout("References")][SerializeField] protected SpriteRenderer _propShadow;
        [Foldout("References")] [SerializeField] protected AudioSource _audioSource;
        [Foldout("References")] [SerializeField] protected Collider _collider;
        
        [Foldout("Debug"), ReadOnly] [SerializeField] private HealthController _healthController;

        [Foldout("Debug"), ReadOnly] [SerializeField]  private StatusContainer _statusContainer;
        
        [SerializeField] private Color _damageColor = Color.white;
        [SerializeField] private Color _critDamageColor = Color.yellow;

        
        [FormerlySerializedAs("destroySfx")] [Foldout("SFX")] [SerializeField] private AudioClip _hurtSFX;
        [FormerlySerializedAs("destroySfx")] [Foldout("SFX")] [SerializeField] private AudioClip _destroySfx;
        
        private Sequence _doSequence;
        
        public bool CanBeDamaged => _healthController.CurrentHealth > 0f && !_healthController.IsImmuneAfterHit;

        public HealthController HealthController => _healthController;
        public StatusContainer StatusContainer => _statusContainer;

        protected virtual void Awake()
        {
            _statusContainer = new StatusContainer(this);
            _healthController = new HealthController(_maxHealth, CancellationToken);
        }
        
        public virtual void AttachListeners()
        {
            _healthController.OnDamaged += OnDamaged;
            _healthController.OnDeath += (this as IDamageable).OnDeath;
        }
        
        protected virtual void Update()
        {
            StatusContainer.UpdateEffects();
        }

        public void TakeDamage(int amount, bool isCrit = false, bool isPercentage = false)
        {
            if (!CanBeDamaged || amount <= 0) return;
            _healthController.TakeDamage(amount, isCrit, isPercentage);
        }

        protected virtual void OnDamaged(int value, bool isCrit)
        {
            if(_propAnimator) _propAnimator?.SetTrigger(AnimationHashes.OnDamaged);

            VFXcontroller.BlinkModelsColor(Color.red, 0.15f, 0.1f, 0.15f);
            VFXController.SpawnAndAttachParticleToEntity(this, VFXcontroller.onHitParticle);
              
            if(_hurtSFX) AudioManager.Instance.PlaySFXAtSource(_hurtSFX, _audioSource, true);
            Color color = isCrit? _critDamageColor : _damageColor;
            PopupDamageManager.Instance?.UsePopup(transform, Quaternion.identity, value, color);
        }

        public void Kill(bool isSilent = false) => _healthController.Kill(isSilent);

        void IDamageable.OnDeath()
        {
            OnDeath();
        }
        
        protected virtual void OnDeath() 
        {
            if(_collider) _collider.enabled = false;
            if(_propAnimator) _propAnimator.SetTrigger(AnimationHashes.Death);

            
            if(_destroySfx) AudioManager.Instance?.PlaySFXAtSource(_destroySfx, _audioSource);
            _propShadow?.DOFade(0, _deathFadeDuration).SetEase(_deathFadeCurve);
            VFXcontroller.DoFadeModel(0f, _deathFadeDuration, _deathFadeCurve);
            VFXController.SpawnParticleInWorld(VFXcontroller.onDeathParticle.particleComponent, transform.position, Quaternion.identity);

            Destroy(gameObject, _deathFadeDuration + 0.5f);
        }
        
        public virtual void DetachListeners()
        {
            _healthController.OnDeath -= (this as IDamageable).OnDeath;
            _healthController.OnDamaged -= OnDamaged;

        }

        protected override void CleanUp()
        {
            base.CleanUp();
            _doSequence?.Kill();
        }
        
#if UNITY_EDITOR
        
        protected override void OnValidate()
        {
            base.OnValidate();
            if(!_collider) _collider = GetComponent<Collider>();
            if(!_audioSource) _audioSource = GetComponent<AudioSource>();
        }
        
#endif

    }
}
