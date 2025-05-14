using UnityEngine;

namespace Terra.AI.EnemyStates {
    /// <summary>
    /// Contains precomputed hash values for the enemy's animations.
    /// Storing hashes helps to improve performance by avoiding repeated
    /// calls to Animator.StringToHash in every state.
    /// </summary>
    public static class EnemyAnimHashes {
        // Precomputed hash for the "IdleNormal" animation state
        public static readonly int Idle = Animator.StringToHash("IdleNormal");

        // Precomputed hash for the "RunFWD" animation state
        public static readonly int Run = Animator.StringToHash("RunFWD");

        // Precomputed hash for the "WalkFWD" animation state
        public static readonly int Walk = Animator.StringToHash("WalkFWD");

        // Precomputed hash for the "Attack01" animation state
        public static readonly int Attack = Animator.StringToHash("Attack01");

        // Precomputed hash for the "Die" animation state
        public static readonly int Die = Animator.StringToHash("Die");
    }
}