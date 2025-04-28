using Terra.Player;
using UnityEngine;

namespace Terra.StateMachine.PlayerStates
{
    public class PlayerBaseState : BaseState
    {
        protected readonly PlayerManager player;
        protected readonly Animator animator;
        
        protected float CrossFadeDuration = 0.1f;

        protected static readonly int IdleUpHash = Animator.StringToHash("IdleUp");
        protected static readonly int IdleDownHash = Animator.StringToHash("IdleDown");
        protected static readonly int IdleLeftHash = Animator.StringToHash("IdleLeft");
        protected static readonly int IdleRightHash = Animator.StringToHash("IdleRight");

        protected static readonly int LocomotionUpHash = Animator.StringToHash("LocomotionUp");
        protected static readonly int LocomotionDownHash = Animator.StringToHash("LocomotionDown");
        protected static readonly int LocomotionLeftHash = Animator.StringToHash("LocomotionLeft");
        protected static readonly int LocomotionRightHash = Animator.StringToHash("LocomotionRight");

        protected static readonly int StunHash = Animator.StringToHash("Stun");

        protected static readonly int DashHash = Animator.StringToHash("Dash");

        protected static readonly int DeathHash = Animator.StringToHash("Death");

        protected static readonly int MeleeAttackUpHash = Animator.StringToHash("MeleeAttackUp");
        protected static readonly int MeleeAttackDownHash = Animator.StringToHash("MeleeAttackDown");
        protected static readonly int MeleeAttackLeftHash = Animator.StringToHash("MeleeAttackLeft");
        protected static readonly int MeleeAttackRightHash = Animator.StringToHash("MeleeAttackRight");

        protected static readonly int RangedAttackUpHash = Animator.StringToHash("RangedAttackUp");
        protected static readonly int RangedAttackDownHash = Animator.StringToHash("RangedAttackDown");
        protected static readonly int RangedAttackLeftHash = Animator.StringToHash("RangedAttackLeft");
        protected static readonly int RangedAttackRightHash = Animator.StringToHash("RangedAttackRight");


        protected PlayerBaseState(PlayerManager player, Animator animator) : base()
        {
            this.player = player;
            this.animator = animator;
        }
    }
}
