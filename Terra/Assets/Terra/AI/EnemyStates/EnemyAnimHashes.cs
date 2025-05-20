using UnityEngine;

namespace Terra.AI.EnemyStates {
    /// <summary>
    /// Contains precomputed hash values for the enemy's animations.
    /// Storing hashes helps to improve performance by avoiding repeated
    /// calls to Animator.StringToHash in every state.
    /// </summary>
    public static class EnemyAnimHashes {
        
        public static readonly int Idle = Animator.StringToHash("Idle");

        public static readonly int WalkLeft = Animator.StringToHash("WalkLeft");
        public static readonly int WalkRight = Animator.StringToHash("WalkRight");


        public static readonly int AttackLeft = Animator.StringToHash("AttackLeft");
        public static readonly int AttackRight = Animator.StringToHash("AttackRight");

        public static readonly int Death = Animator.StringToHash("Death");
        
    }
}