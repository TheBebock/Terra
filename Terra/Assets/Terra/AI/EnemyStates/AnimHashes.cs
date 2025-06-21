using UnityEngine;

namespace Terra.AI.EnemyStates {
    /// <summary>
    ///     Contains precomputed hash values for animations.
    /// </summary>
    public static class AnimationHashes {
        
        public static readonly int Default = Animator.StringToHash("Default");
        public static readonly int Idle = Animator.StringToHash("Idle");
        public static readonly int OnDamaged = Animator.StringToHash("OnDamaged");

        public static readonly int WalkLeft = Animator.StringToHash("WalkLeft");
        public static readonly int WalkRight = Animator.StringToHash("WalkRight");

        public static readonly int AttackDown = Animator.StringToHash("AttackDown");
        public static readonly int AttackUp = Animator.StringToHash("AttackUp");
        public static readonly int AttackLeft = Animator.StringToHash("AttackLeft");
        public static readonly int AttackRight = Animator.StringToHash("AttackRight");

        public static readonly int Death = Animator.StringToHash("Death");        
        public static readonly int DeathLeft = Animator.StringToHash("DeathLeft");        
        public static readonly int DeathRight = Animator.StringToHash("DeathRight");        
        public static readonly int Direction = Animator.StringToHash("Direction");
        
    }
}