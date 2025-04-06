using NaughtyAttributes;
using System.Collections;
using Terra.Core.Generics;
using Terra.InputManagement;
using Terra.Itemization.Items.Definitions;
using Terra.Player;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackManager : MonoBehaviourSingleton<PlayerAttackManager>
{
    private InputSystem.PlayerControlsActions inputActions;
    private PlayerManager _playerManager;

    public bool IsTryingPerformMeleeAttack = false;
    public bool IsTryingPerformDistanceAttack = false;

    [SerializeField, ReadOnly] private float currentMeleeCD = 0;
    [SerializeField, ReadOnly] private float currentRangedCD = 0;

    private float maxMeleeCD = 0;
    private float maxRangedCD = 0;

    public float CurrentMeleeCooldown => currentMeleeCD;
    public float CurrentRangedCooldown => currentRangedCD;

        private void Start()
    {
        inputActions = InputManager.Instance.PlayerControls;
        _playerManager = PlayerManager.Instance;
    }

    public IEnumerator DecreaseMeleeCooldown()
    {
        while(CurrentMeleeCooldown > 0)
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
            IsTryingPerformMeleeAttack = true;
            currentMeleeCD = maxMeleeCD;
            StartCoroutine(DecreaseMeleeCooldown());
        }
    }

    private void OnDistanceAttackInput(InputAction.CallbackContext context)
    {
        maxRangedCD = _playerManager.PlayerInventory.GetRangedWeapon.Data.attackSpeed;
        if (currentRangedCD == 0)
        {
            IsTryingPerformDistanceAttack = true;
            currentRangedCD = maxRangedCD;
            StartCoroutine(DecreaseRangedCooldown());
        }
    }

        public void AttachListeners()
    {
        inputActions.MelleAttack.performed += OnMeleeAttackInput;
        inputActions.DistanceAttack.performed += OnDistanceAttackInput;
    }

    public void DetachListeners()
    {
        if (inputActions.MelleAttack != null)
            inputActions.MelleAttack.performed -= OnMeleeAttackInput;

        if (inputActions.DistanceAttack != null)
            inputActions.Dash.performed -= OnDistanceAttackInput;
    }
}