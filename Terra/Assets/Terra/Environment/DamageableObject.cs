using DG.Tweening;
using NaughtyAttributes;
using Terra.AI.EnemyStates;
using Terra.Combat;
using Terra.Core.ModifiableValue;
using Terra.EffectsSystem;
using Terra.Interactions;
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
    public class DamageableObject : InteractableBase, IDamageable, IAttachListeners
    {
        [SerializeField] private ModifiableValue _maxHealth;
        [SerializeField] private float _deathFadeDuration = 2.5f;
        [SerializeField] private AnimationCurve _deathFadeCurve;
        
        [Foldout("References")][SerializeField] private Animator _propAnimator;
        [Foldout("References")][SerializeField] private SpriteRenderer _propShadow;
        [Foldout("References")] [SerializeField] private AudioSource _audioSource;
        [Foldout("References")] [SerializeField] private Collider _collider;
        
        [Foldout("Debug"), ReadOnly] [SerializeField] private HealthController _healthController;
        [Foldout("Debug"), ReadOnly] [SerializeField] private StatusContainer _statusContainer;
        
        [FormerlySerializedAs("destroySfx")] [Foldout("SFX")] [SerializeField] private AudioClip _destroySfx;
        
        private Sequence _doSequence;

        
        public bool IsInvincible => _healthController.IsInvincible;
        public bool CanBeDamaged => _healthController.CurrentHealth > 0f && !_healthController.IsImmuneAfterHit;

        public HealthController HealthController => _healthController;
        public StatusContainer StatusContainer => _statusContainer;

        protected virtual void Awake()
        {
            _statusContainer = new StatusContainer(this);
            _healthController = new HealthController(_maxHealth, CancellationToken);
        }

        protected virtual void Update()
        {
            StatusContainer.UpdateEffects();
        }

        public void TakeDamage(int amount, bool isPercentage = false)
        {
            if (!CanBeDamaged) return;
            _healthController.TakeDamage(amount, isPercentage);
        }

        protected virtual void OnDamaged(int value)
        {
            _propAnimator?.SetTrigger(AnimationHashes.OnDamaged);

            VFXcontroller.BlinkModelsColor(Color.red, 0.15f, 0.1f, 0.15f);
            VFXController.SpawnAndAttachParticleToEntity(this, VFXcontroller.onHitParticle);
              
            // Show VFX
            PopupDamageManager.Instance.UsePopup(transform, Quaternion.identity, value);
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

            
            AudioManager.Instance.PlaySFXAtSource(_destroySfx, _audioSource);
            _propShadow?.DOFade(0, _deathFadeDuration).SetEase(_deathFadeCurve);
            VFXcontroller.DoFadeModel(0f, _deathFadeDuration, _deathFadeCurve);
            VFXController.SpawnAndAttachParticleToEntity(this, VFXcontroller.onDeathParticle);

            Destroy(gameObject, _deathFadeDuration + 0.5f);
        }
        
        public override void OnInteraction()
        {
            //DO nothing
        }

        public virtual void AttachListeners()
        {
            _healthController.OnDeath += (this as IDamageable).OnDeath;
            _healthController.OnDamaged += OnDamaged;

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

        protected override void OnValidate()
        {
            base.OnValidate();
            if(!_collider) _collider = GetComponent<Collider>();
            if(!_audioSource) _audioSource = GetComponent<AudioSource>();
        }
    }
}
