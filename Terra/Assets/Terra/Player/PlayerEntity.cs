using NaughtyAttributes;
using Terra.Combat;
using Terra.Core.Generics;
using Terra.EffectsSystem;
using Terra.Interfaces;
using Terra.UI.HUD;
using UnityEngine;

namespace Terra.Player
{

    /// <summary>
    ///     Class represents player entity inside the game world
    /// </summary>
    public class PlayerEntity : Entity, IDamageable, IHealable, IWithSetUp
    {
        [Foldout("Debug"), ReadOnly] [SerializeField]
        private HealthController _healthController;
        [Foldout("Debug"), ReadOnly] [SerializeField]
        private StatusContainer _statusContainer;
        public bool CanBeDamaged { get; set; } = true;
        public bool CanBeHealed => _healthController.CanBeHealed;
        public bool IsInvincible => _healthController.IsInvincible;
        public float MaxHealth => _healthController.MaxHealth;
        public float CurrentHealth => _healthController.CurrentHealth;

        public StatusContainer StatusContainer => _statusContainer;
        public HealthController HealthController => _healthController;

        public override int Identity => PlayerManager.Instance.Identity;

        public override void RegisterID()
        {
            //Do not register
        }

        public override void ReturnID()
        {
            //Do not return
        }

        public void SetUp()
        {
            _statusContainer = new StatusContainer(this);
            _healthController = new HealthController(PlayerStatsManager.Instance.PlayerStats.MaxHealth, true);
            _healthController.OnDeath += (this as IDamageable).OnDeath;
            HPSlider.Instance.Init(_healthController.CurrentHealth, _healthController.MaxHealth);
            CanBeDamaged = true;
        }

        private void Update()
        {
            StatusContainer.UpdateEffects();
        }

        public void TakeDamage(float amount, bool isPercentage = false)
        {
            if (!CanBeDamaged) return;
            Debug.Log($"{this}: Taking Damage {amount}");
            _healthController.TakeDamage(amount, isPercentage);
            PopupDamageManager.Instance.UsePopup(transform.position, Quaternion.identity, amount);
        }

        public void Heal(float amount, bool isPercentage = false)
        {
            if(!CanBeHealed) return;
            _healthController.Heal(amount, isPercentage);
        }

        void IDamageable.OnDeath()
        {
            OnDeath();
        }
        private void OnDeath()
        {
            CanBeDamaged = false;
            PlayerManager.Instance.OnPlayerDeathNotify();
        }
        
        public void ResetHealth(bool isSilent = true) => _healthController.ResetHealth(isSilent);
        public void Kill(bool isSilent = true) => _healthController.Kill(isSilent);
        
        
        public void TearDown()
        {
            _healthController.OnDeath -= (this as IDamageable).OnDeath;
        }
    }
}