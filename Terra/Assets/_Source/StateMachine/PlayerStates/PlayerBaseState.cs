using System.Collections;
using System.Collections.Generic;
using Terra.Player;
using Terra.StateMachine;
using UnityEngine;

public class PlayerBaseState : BaseState
{
    protected readonly PlayerManager player;
    protected readonly Animator animator;
        
    protected static readonly int LocomotionHash = Animator.StringToHash("Locomotion");
    protected static readonly int StunHash = Animator.StringToHash("Stun");
    
    protected PlayerBaseState(PlayerManager player, Animator animator) : base()
    {
        this.player = player;
        this.animator = animator;
    }
}
