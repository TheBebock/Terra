using System;
using Core.ModifiableValue;
using DG.Tweening;
using Terra.Combat;
using Terra.Interfaces;
using Terra.Managers;
using UnityEngine;

namespace Terra.Environment
{

    /// <summary>
    /// Represents object in game world that can be damaged
    /// </summary>
    public class DamageableObject : InteractableBase, IDamagable, IAttachListeners
    {
        [SerializeField] private ModifiableValue maxHealth;
        [SerializeField] private SpriteRenderer propModel;

        [Header("On Damage VFX")] 
        [SerializeField] private Vector3 scale;
        [SerializeField] private Vector3 moveOffset;
        [SerializeField] private Color damagedColor = Color.red;
        
        private HealthController _healthController;
        public bool IsInvincible => _healthController.IsInvincible;
        public bool CanBeDamaged => _healthController.CurrentHealth > 0;
        public override bool CanBeInteractedWith { get; protected set; }
    
        public HealthController HealthController => _healthController;
        
        private Sequence _moveSequence;

        private void Awake()
        {
            _healthController = new HealthController(maxHealth);
        }

        public void TakeDamage(float amount)
        {
            if (!CanBeDamaged) return;
            _healthController.TakeDamage(amount);
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

        public virtual void OnDeath()
        {
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
            _healthController.OnDeath += OnDeath;
            _healthController.OnDamaged += OnDamaged;

        }

        public virtual void DetachListeners()
        {
            _healthController.OnDeath -= OnDeath;
            _healthController.OnDamaged -= OnDamaged;

        }

        protected override void CleanUp()
        {
            base.CleanUp();
            _moveSequence?.Kill();
        }
    }
}
