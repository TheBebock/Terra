using System.Collections;
using System.Collections.Generic;
using Core.Generics;
using NUnit.Framework.Constraints;
using UnityEngine;

public class InputManager : MonoBehaviourSingleton<InputManager>
{
    [SerializeField] private InputSystem inputActions;
    private bool isPlayerAlive = true;
    private bool canPlayerMove = true;

    protected override void Awake()
    {
        base.Awake();
        if (inputActions == null)
        {
            inputActions = new InputSystem();
        }
        EnableAllTimeActions(); // Activate global actions
    }

    public InputSystem GetInputActions()
    {
        return inputActions;
    }

    private void EnableAllTimeActions()
    {
        inputActions.AllTime.Enable(); 
    }

    public void EnablePlayerControls()
    {
        if (inputActions?.PlayerControls == null)
        {
            Debug.LogError("InputActions or PlayerControls is null in EnablePlayerControls.");
            return;
        }
        inputActions.PlayerControls.Enable();
    }

    public void DisablePlayerControls()
    {
        if (inputActions?.PlayerControls == null)
        {
            Debug.LogWarning("PlayerControls is null in DisablePlayerControls.");
            return;
        }
        inputActions.PlayerControls.Disable(); 
    }
    /// <summary>
    /// for now SetPlayerAlive and SetPlayerCanMove are not used neither in InputManager nor PlayerMovement
    /// but i leave them for now as they might be useful in the future to things such as
    /// blocking animation, changing the status of health bar etc. I will set them as protected
    /// in order to not cause any bugs in the near future
    /// </summary>
    
    protected void SetPlayerAlive(bool state) 
    {
        isPlayerAlive = state;
        UpdatePlayerControlsState();
    }

    protected void SetPlayerCanMove(bool state)
    {
        canPlayerMove = state;
        UpdatePlayerControlsState();
    }
    
    
    private void UpdatePlayerControlsState()
    {
        if (isPlayerAlive && canPlayerMove)
        {
            EnablePlayerControls();
        }
        else
        {
            DisablePlayerControls();
        }
    }

    private void HandleInputState()
    {
        UpdatePlayerControlsState();
    }

    public bool IsPlayerAlive()
    {
        return isPlayerAlive;
    }

    public bool CanPlayerMove()
    {
        return canPlayerMove;
    }

    void Start()
    {
        UpdatePlayerControlsState();
    }

    void Update()
    {
        UpdatePlayerControlsState();
    }
}
