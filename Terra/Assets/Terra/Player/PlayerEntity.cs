using NaughtyAttributes;
using Terra.Combat;
using Terra.Core.Generics;
using Terra.EffectsSystem;
using Terra.EventsSystem;
using Terra.EventsSystem.Events;
using Terra.Interfaces;
using Terra.Managers;
using Terra.Particles;
using Terra.UI.HUD;
using UnityEngine;

namespace Terra.Player
{

    /// <summary>
    ///     Class represents player entity inside the game world
    /// </summary>
    public class PlayerEntity : Entity, IDamageable, IHealable, IWithSetUp, IAttachListeners
    {
        [Header("SFX")]
        [SerializeField] private AudioClip _hurtSound;
        [SerializeField] private AudioClip _deathSound;
        [SerializeField] private AudioClip _dashSound;
        
        [Header("Pop up colors")]
        [SerializeField] private Color _onDamagedColor = Color.red;
        [SerializeField] private Color _onHealedColor = Color.green;
        
        [Foldout("References")] [SerializeField]
        private Collider _collider;        
        [Foldout("References")] [SerializeField]
        private AudioSource _audioSource;
        [Foldout("Debug"), ReadOnly] [SerializeField]
        private HealthController _healthController;
        [Foldout("Debug"), ReadOnly] [SerializeField]
        private StatusContainer _statusContainer;
        
        public bool CanBeDamaged => _healthController.CurrentHealth > 0f && !_healthController.IsImmuneAfterHit;
        public bool CanBeHealed => _healthController.CanBeHealed;
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
            _healthController = new HealthController(PlayerStatsManager.Instance.PlayerStats.MaxHealthValue, CancellationToken, true);
            _healthController.OnDeath += (this as IDamageable).OnDeath;
            _healthController.OnDamaged += OnDamaged;
            _healthController.OnHealed += OnHealed;
            HUDManager.Instance.HpSlider.Init(_healthController.CurrentHealth, _healthController.MaxHealth);
        }
        
        public void AttachListeners()
        {
          
            EventsAPI.Register<OnPlayerDashStartedEvent>(OnPlayerDashStarted);
            EventsAPI.Register<OnPlayerDashEndedEvent>(OnPlayerDashEnded);
            if (PlayerStatsManager.Instance)
            {
                PlayerStatsManager.Instance.OnMaxHealthChanged += OnMaxHealthChanged;
            }
        }
        
        private void Update()
        {
            StatusContainer.UpdateEffects();
        }

        private void OnMaxHealthChanged(int newMaxHealth)
        {
            int difference = newMaxHealth - _healthController.PreviousMaxHealth;
            _healthController.SetPreviousMaxHealth(newMaxHealth);

            if (difference > 0)
            {
                _healthController.Heal(difference, isSilent:true);
            }
            else
            {
                if (_healthController.CurrentHealth > _healthController.MaxHealth)
                {
                    _healthController.ResetHealth();
                }
            }
        }
        
        public void TakeDamage(int amount, bool isCrit = false, bool isPercentage = false)
        {
            if (!CanBeDamaged || amount <= 0) return;
            Debug.Log($"{this}: Taking Damage {amount}");
            _healthController.TakeDamage(amount, isCrit, isPercentage);
        }

        private void OnDamaged(int amount, bool isCrit = false)
        {
            PopupDamageManager.Instance.UsePopup(transform, Quaternion.identity, amount, _onDamagedColor);
            if(_hurtSound) AudioManager.Instance?.PlaySFXAtSource(_hurtSound, _audioSource, true);
            
            VFXcontroller.BlinkModelsColor(Color.red, 0.1f,0.1f,0.1f);
            VFXController.SpawnAndAttachParticleToEntity(this, VFXcontroller.onHitParticle);
        }
        public void Heal(int amount, bool isPercentage = false)
        {
            if(!CanBeHealed || amount <=0) return;
            _healthController.Heal(amount, isPercentage);
        }

        private void OnHealed(int amount)
        {
            PopupDamageManager.Instance.UsePopup(transform, Quaternion.identity, amount, _onHealedColor);
            
            VFXcontroller.BlinkModelsColor(Color.green, 0.15f, 0.1f, 0.15f);
            VFXController.SpawnAndAttachParticleToEntity(this, VFXcontroller.onHealParticle);
        }
        
        
        void IDamageable.OnDeath()
        {
            OnDeath();
        }
        private void OnDeath()
        {
            PlayerManager.Instance?.OnPlayerDeathNotify();
            if(_deathSound) AudioManager.Instance?.PlaySFXAtSource(_deathSound, _audioSource);

        }

        private void OnPlayerDashStarted(ref OnPlayerDashStartedEvent dashEvent)
        {
            _collider.enabled = false;
            AudioManager.Instance?.PlaySFXAtSource(_dashSound, _audioSource);
        }
        
        private void OnPlayerDashEnded(ref OnPlayerDashEndedEvent dashEvent)
        {
            _collider.enabled = true;
        }
        
        public void ResetHealth(bool isSilent = true) => _healthController.ResetHealth(isSilent);
        public void Kill(bool isSilent = true) => _healthController.Kill(isSilent);
        
        
        public void TearDown()
        {
            _healthController.OnDeath -= (this as IDamageable).OnDeath;
            _healthController.OnDamaged -= OnDamaged;
            _healthController.OnHealed -= OnHealed;
        }

        public void DetachListeners()
        {
            EventsAPI.Unregister<OnPlayerDashStartedEvent>(OnPlayerDashStarted);
            EventsAPI.Unregister<OnPlayerDashEndedEvent>(OnPlayerDashEnded);
            if (PlayerStatsManager.Instance)
            {
                PlayerStatsManager.Instance.OnMaxHealthChanged -= OnMaxHealthChanged;
            }
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            if(!_audioSource) _audioSource = GetComponent<AudioSource>();
        }
#endif
       
    }
}