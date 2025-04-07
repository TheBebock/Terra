using System;
using NaughtyAttributes;
using System.Collections;
using Terra.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Terra.Player
{

    /// <summary>
    /// Handles player attacks
    /// </summary>
    [Serializable]
    public class PlayerAttackController : IAttachListeners
    {
        private InputSystem.PlayerControlsActions inputActions;
        [Foldout("Debug"), ReadOnly] [SerializeField]
        
        private PlayerManager _playerManager;

        [Foldout("Debug"), ReadOnly] [SerializeField]
        private bool isTryingPerformMeleeAttack = false;

        [Foldout("Debug"), ReadOnly] [SerializeField]
        private bool isTryingPerformDistanceAttack = false;


        [Foldout("Debug"), ReadOnly] [SerializeField]
        private float currentMeleeCD = 0;

        [Foldout("Debug"), ReadOnly] [SerializeField]
        private float currentRangedCD = 0;

        [Foldout("Debug"), ReadOnly] [SerializeField]
        private float maxMeleeCD = 0;

        [Foldout("Debug"), ReadOnly] [SerializeField]
        private float maxRangedCD = 0;

        public bool IsTryingPerformMeleeAttack
        {
            get => isTryingPerformMeleeAttack;
            set => isTryingPerformMeleeAttack = value;

        }

        public bool IsTryingPerformDistanceAttack
        {
            get => isTryingPerformDistanceAttack;
            set => isTryingPerformDistanceAttack = value;
        }

        public float CurrentMeleeCooldown => currentMeleeCD;
        public float CurrentRangedCooldown => currentRangedCD;


        public PlayerAttackController(InputSystem.PlayerControlsActions playerControls, PlayerManager playerManager)
        {
            inputActions = playerControls;
            _playerManager = playerManager;

            AttachListeners();
        }


        public void AttachListeners()
        {
            inputActions.MelleAttack.performed += OnMeleeAttackInput;
            inputActions.DistanceAttack.performed += OnDistanceAttackInput;
        }

        public IEnumerator DecreaseMeleeCooldown()
        {
            while (CurrentMeleeCooldown > 0)
            {
                currentMeleeCD--;
                yield return new WaitForSeconds(0.1f);
            }
        }

        public IEnumerator DecreaseRangedCooldown()
        {
            while (CurrentRangedCooldown > 0)
            {
                currentRangedCD--;
                yield return new WaitForSeconds(0.1f);
            }
        }

        private void OnMeleeAttackInput(InputAction.CallbackContext context)
        {
            maxMeleeCD = _playerManager.PlayerInventory.GetMeleeWeapon.Data.attackSpeed;
            if (currentMeleeCD == 0)
            {
                isTryingPerformMeleeAttack = true;
                currentMeleeCD = maxMeleeCD;
                _playerManager.StartCoroutine(DecreaseMeleeCooldown());
            }
        }

        private void OnDistanceAttackInput(InputAction.CallbackContext context)
        {
            maxRangedCD = _playerManager.PlayerInventory.GetRangedWeapon.Data.attackSpeed;
            if (currentRangedCD == 0)
            {
                isTryingPerformDistanceAttack = true;
                currentRangedCD = maxRangedCD;
                _playerManager.StartCoroutine(DecreaseRangedCooldown());
            }
        }


        public void DetachListeners()
        {
            if (inputActions.MelleAttack != null)
                inputActions.MelleAttack.performed -= OnMeleeAttackInput;

            if (inputActions.DistanceAttack != null)
                inputActions.Dash.performed -= OnDistanceAttackInput;
        }
        
    }
}