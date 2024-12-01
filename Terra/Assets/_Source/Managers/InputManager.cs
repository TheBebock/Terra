using System.Collections;
using System.Collections.Generic;
using Core.Generics;
using NUnit.Framework.Constraints;
using UnityEngine;

public class InputManager : MonoBehaviourSingleton<InputManager>
{
    
    // Start is called before the first frame update
    private InputSystem inputActions;
    private bool isPlayerAlive = true;
    private bool canPlayerMove = true;
    
    
    private void Awake()
    {
        inputActions = new InputSystem(); 
        inputActions.Enable(); 
    }

    
    public InputSystem GetInputActions()
    {
        if (!isPlayerAlive)
        {
            Debug.LogWarning("Player is dead. Disabling input.");
            inputActions.Disable();
        }
        else if (!canPlayerMove)
        {
            Debug.LogWarning("Player cannot move. Disabling movement input.");
            inputActions.Disable();
        }
        else
        {
            inputActions.Enable();
        }

        return inputActions;
    }
    
    
    private void HandleInputState()
    {
        if (isPlayerAlive && canPlayerMove)
        {
            inputActions.Enable();
        }
        else
        {
            inputActions.Disable(); 
        }
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
        HandleInputState();
    }

    // Update is called once per frame
    void Update()
    {
        HandleInputState();
    }
}
