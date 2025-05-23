using DG.Tweening;
using NaughtyAttributes;
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
        [SerializeField] private ModifiableValue maxHealth;
        [SerializeField] private SpriteRenderer propModel;

        [Header("On Damage VFX")] 
        [SerializeField] private Vector3 scale;
        [SerializeField] private Vector3 moveOffset;
        [SerializeField] private Color damagedColor = Color.red;
        
        [Foldout("Debug"), ReadOnly] [SerializeField] private HealthController _healthController;
        [Foldout("Debug"), ReadOnly] [SerializeField] private StatusContainer _statusContainer;
        
        [Foldout("SFX")] [SerializeField] public AudioClip destroySfx;
        public bool IsInvincible => _healthController.IsInvincible;
        public bool CanBeDamaged => _healthController.CurrentHealth > 0;
        public override bool CanBeInteractedWith { get; protected set; }

        

        public HealthController HealthController => _healthController;
        public StatusContainer StatusContainer => _statusContainer;

        private Sequence _moveSequence;
        
        private AudioSource audioSource;

        protected virtual void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            _statusContainer = new StatusContainer(this);
            _healthController = new HealthController(maxHealth);
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
        }

        private void OnDamaged(float value)
        {
            propModel.material.DOColor(damagedColor, 0.25f)
                .SetLoops(2, LoopType.Yoyo);
            propModel.transform.DOScale(scale, 0.25f)
                .SetLoops(2, LoopType.Yoyo);
            
            Vector3 startPos = propModel.transform.localPosition;
            

            _moveSequence
                .Append(propModel.transform.DOLocalMove(-moveOffset, 0.2f).SetRelative())
                .Append(propModel.transform.DOLocalMove(moveOffset * 2f, 0.2f).SetRelative())
                .Append(propModel.transform.DOLocalMove(startPos, 0.2f));
        }

        public void Kill(bool isSilent = false) => _healthController.Kill(isSilent);

        void IDamageable.OnDeath()
        {
            OnDeath();
        }
        protected virtual void OnDeath()
        {
            AudioManager.Instance.PlaySFXAtSource(destroySfx, audioSource);
            propModel.material.DOFade(0f, 2.5f);
            
            LootManager.Instance.SpawnRandomItem(transform.position);
            
            Destroy(gameObject, 3f);

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
            _moveSequence?.Kill();
        }
        
    }
}
