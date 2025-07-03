using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using Terra.AI.EnemyStates;
using Terra.Core.Generics;
using Terra.Enums;
using Terra.EventsSystem;
using Terra.EventsSystem.Events;
using Terra.Interfaces;
using Terra.Itemization.Items;
using Terra.Player;
using UnityEngine;

namespace Terra.Combat
{
    public class AttackHitboxScaler : InGameMonobehaviour, IAttachListeners, IWithSetUp
    {
        [Foldout("References")] [SerializeField]
        private Entity _entity;
        
        [Foldout("Debug"), ReadOnly] [SerializeField]
        private Transform _heldHitbox;
        
        [Foldout("Debug"), ReadOnly] [SerializeField]
        private List<WeaponCollider> _weaponColliders;
    
        [Foldout("Debug"), ReadOnly] [SerializeField]
        private Animator _hitboxAnimator;
    
        [Foldout("Debug"), ReadOnly] [SerializeField]
        private float _positionToScaleRatio;

        [Foldout("Debug"), ReadOnly] [SerializeField]
        private Vector3 _originalScale;

        [Foldout("Debug"), ReadOnly] [SerializeField]
        private Vector3 _originalPosition;
    
        public void SetUp()
        {
            UpdateHeldHitbox(PlayerInventoryManager.Instance.MeleeWeapon);
        }
        
        public void AttachListeners() 
        {
            if (PlayerInventoryManager.Instance)
            {
                PlayerInventoryManager.Instance.OnMeleeWeaponChanged += UpdateHeldHitbox;
            }

            
            if (PlayerManager.Instance)
            {
                EventsAPI.Register<OnPlayerMeleeAttackPerformedEvent>(StartAttack);
            }
        }
        
        private void StartAttack(ref OnPlayerMeleeAttackPerformedEvent meleeEvent)
        {
            float scaleModif = Mathf.InverseLerp(-100f, 100f, PlayerStatsManager.Instance.PlayerStats.MeleeRange);
            ScaleHitbox(Mathf.Lerp(0.5f, 1.5f, scaleModif));

            _heldHitbox.gameObject.SetActive(true);

            var animationSpeedModifier = Mathf.Clamp01(PlayerStatsManager.Instance.PlayerStats.SwingSpeed);
            _hitboxAnimator.SetFloat("SlashSpeed", Mathf.Lerp(0.8f, 1.6f, animationSpeedModifier));
            
            _hitboxAnimator.Play(GetAnimationHash(meleeEvent.facingDirection), -1, 0f);

            _ = WaitForAnimationEnd().AttachExternalCancellation(destroyCancellationToken);
        }
        
        private void UpdateHeldHitbox(MeleeWeapon meleeWeapon)
        {
            if (_heldHitbox)
            {
                Destroy(_heldHitbox.gameObject);
            }
            
            if (meleeWeapon == null)
            {
                Debug.LogError($"{this}: New hitbox is null");
                return;
            }

            _heldHitbox = Instantiate( meleeWeapon.Data.attackPrefab.transform, transform);

            _weaponColliders.Clear();
            _weaponColliders = _heldHitbox.GetComponentsInChildren<WeaponCollider>().ToList();

            for (int i = 0; i < _weaponColliders.Count; i++)
            {
                _weaponColliders[i].Init(_entity, meleeWeapon.Data);
            }
            
            _hitboxAnimator = _heldHitbox.gameObject.GetComponent<Animator>();   
            _originalPosition = _heldHitbox.localPosition;
            _originalScale = _heldHitbox.localScale;
            
            UpdateScaleRatio();
            _heldHitbox.gameObject.SetActive(false);
        }

        private int GetAnimationHash(FacingDirection facingDirection)
        {
            switch (facingDirection)
            {
                case FacingDirection.Up:
                    return AnimationHashes.AttackUp;
                case FacingDirection.Down:
                    return AnimationHashes.AttackDown;
                case FacingDirection.Left:
                    return AnimationHashes.AttackLeft;
                case FacingDirection.Right:
                    return AnimationHashes.AttackRight;
            }
            
            return AnimationHashes.AttackDown;
        }
        
        private async UniTask WaitForAnimationEnd()
        {
            while (_hitboxAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.95f)
                await UniTask.Yield();
            DisableHitboxes();
        }

        private void DisableHitboxes()
        {
            _heldHitbox.gameObject.SetActive(false);
        }
        
        private void ScaleHitbox(float scaleModifier = 0f)
        {
            if (scaleModifier == 0) return;
        
            float positionModifier = scaleModifier;
        
            Vector3 newScale = _originalScale + _originalScale * scaleModifier;
            Vector3 newPosition = _originalPosition + _originalPosition * positionModifier;

            _heldHitbox.transform.localScale = newScale;
            _heldHitbox.transform.localPosition = newPosition;
        }
        private void UpdateScaleRatio()
        {
            _positionToScaleRatio = _heldHitbox.transform.localPosition.z / _heldHitbox.transform.localScale.x;
        }
        
        public void DetachListeners()
        {
            if (PlayerInventoryManager.Instance)
            {
                PlayerInventoryManager.Instance.OnMeleeWeaponChanged -= UpdateHeldHitbox;
            }
            
            EventsAPI.Unregister<OnPlayerMeleeAttackPerformedEvent>(StartAttack);
        }
        public void TearDown()
        {
        }
    }
}
