using NaughtyAttributes;
using Terra.Combat;
using Terra.Core.Generics;
using Terra.Interfaces;
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

        public bool CanBeDamaged { get; set; } = true;
        public bool CanBeHealed => _healthController.CanBeHealed;
        public bool IsInvincible => _healthController.IsInvincible;
        public float MaxHealth => _healthController.MaxHealth;
        public float CurrentHealth => _healthController.CurrentHealth;


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
            _healthController = new HealthController(PlayerStatsManager.Instance.PlayerStats.MaxHealth, true);
            _healthController.OnDeath += (this as IDamageable).OnDeath;
            CanBeDamaged = true;
        }

        public void TakeDamage(float amount, bool isPercentage = false)
        {
            if (!CanBeDamaged) return;
            _healthController.TakeDamage(amount, isPercentage);
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