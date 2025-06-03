using System;
using DG.Tweening;
using NaughtyAttributes;
using Terra.AI.EnemyStates;
using Terra.Combat;
using Terra.Core.ModifiableValue;
using Terra.EffectsSystem;
using Terra.Interactions;
using Terra.Interfaces;
using Terra.Managers;
using UnityEngine;

namespace Terra.Environment
{

    /// <summary>
    /// Represents object in game world that can be damaged
    /// </summary>
    public class DamageableObject : InteractableBase, IDamageable, IAttachListeners
    {
        [SerializeField] private ModifiableValue _maxHealth;
        [SerializeField, Min(0.1f)] private float _spawnItemOffset = 1f;
        [SerializeField] private Color _onDamagedColor = Color.red;
        [SerializeField] private float _deathFadeDuration = 2.5f;
        [SerializeField] private AnimationCurve _deathFadeCurve;
        
        [Foldout("References")][SerializeField] private SpriteRenderer _propModel;
        [Foldout("References")][SerializeField] private Animator _propAnimator;
        [Foldout("References")][SerializeField] private SpriteRenderer _propShadow;
        
        [Foldout("Debug"), ReadOnly] [SerializeField] private AudioSource _audioSource;
        [Foldout("Debug"), ReadOnly] [SerializeField] private Collider _collider;
        [Foldout("Debug"), ReadOnly] [SerializeField] private HealthController _healthController;
        [Foldout("Debug"), ReadOnly] [SerializeField] private StatusContainer _statusContainer;
        
        [Foldout("SFX")] [SerializeField] public AudioClip destroySfx;
        
        
        private Sequence _doSequence;
        
        
        
        public bool IsInvincible => _healthController.IsInvincible;
        public bool CanBeDamaged => _healthController.CurrentHealth > 0f && !_healthController.IsImmuneAfterHit;
        public override bool CanBeInteractedWith { get; protected set; }
        
        public HealthController HealthController => _healthController;
        public StatusContainer StatusContainer => _statusContainer;
        protected Vector3 SpawnLootOffset => new(
            transform.position.x, 
            transform.position.y, 
            transform.position.z - _spawnItemOffset);

        protected virtual void Awake()
        {
            _statusContainer = new StatusContainer(this);
            _healthController = new HealthController(_maxHealth, CancellationToken);
        }

        protected virtual void Update()
        {
            StatusContainer.UpdateEffects();
        }

        public void TakeDamage(float amount, bool isPercentage = false)
        {
            if (!CanBeDamaged) return;
            _healthController.TakeDamage(amount, isPercentage);
            
            // Show VFX
            PopupDamageManager.Instance.UsePopup(transform, Quaternion.identity, amount);
        }

        protected virtual void OnDamaged(float value)
        {
            
            _propAnimator.SetTrigger(AnimationHashes.OnDamaged);

            if (_doSequence == null)
            {
                _doSequence.Append(_propModel.material.DOColor(_onDamagedColor, 0.25f)
                    .SetLoops(2, LoopType.Yoyo));
            }
        }

        public void Kill(bool isSilent = false) => _healthController.Kill(isSilent);

        void IDamageable.OnDeath()
        {
            OnDeath();
        }
        protected virtual void OnDeath()
        {
            _collider.enabled = false;
            _propShadow.enabled = false;

            AudioManager.Instance.PlaySFXAtSource(destroySfx, _audioSource);
            _propAnimator.SetTrigger(AnimationHashes.Death);
            _propModel.material.DOFade(0f, _deathFadeDuration).SetEase(_deathFadeCurve);
           
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

        private void OnValidate()
        {
            if(!_collider) _collider = GetComponent<Collider>();
            if(!_audioSource) _audioSource = GetComponent<AudioSource>();
        }
    }
}
