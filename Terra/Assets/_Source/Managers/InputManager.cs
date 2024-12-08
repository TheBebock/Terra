using System.Collections;
using System.Collections.Generic;
using Core.Generics;
using NUnit.Framework.Constraints;
using UnityEngine;

public class InputManager : MonoBehaviourSingleton<InputManager>
{
    private InputSystem inputActions;
    
    //This will be moved to separe class regarding player states
    private bool canPlayerMove = true;

    
    public InputSystem GetInputActions() => inputActions;
    
    protected override void Awake()
    {
        base.Awake();
        if (inputActions == null)
        {
            inputActions = new InputSystem();
        }
        EnableAllTimeActions(); // Activate global actions
        EnablePlayerControls();
    }
    
    void Start()
    {
        UpdatePlayerControlsState(true);
    }
    
    private void UpdatePlayerControlsState(bool value)
    {
        if (value)
        {
            EnablePlayerControls();
        }
        else
        {
            DisablePlayerControls();
        }
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
    
    protected void SetPlayerCanMove(bool state)
    {
        canPlayerMove = state;
        UpdatePlayerControlsState(state);
    }
    
    


    public bool CanPlayerMove()
    {
        return canPlayerMove;
    }


}

