using Terra.AI.Enemy;
using UnityEngine;
using UnityEngine.AI;

namespace Terra.AI.EnemyStates
{
    public class EnemyDeathState : EnemyBaseState
    {
        
        private bool _hasPlayedDeathAnimation;
        public EnemyDeathState(EnemyBase enemy, NavMeshAgent agent, Animator animator) : base(enemy, agent, animator)
        {
        }

        // Called when entering the death state
        public override void OnEnter()
        {
            // Check if the death animation is already playing
            if (_hasPlayedDeathAnimation || animator.GetCurrentAnimatorStateInfo(0).IsName("Die"))
                return;

            // Play the death animation smoothly
            base.OnEnter();
            animator.CrossFade(AnimationHashes.Death, CrossFadeDuration);

            // Set the flag to prevent the animation from being played again
            _hasPlayedDeathAnimation = true;
        }

        // Update method called each frame to check if the death animation is finished
        public override void Update()
        {
            // If the death animation is finished (normalizedTime >= 1f means the animation has played fully)
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Die") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                // Logic to execute after the animation finishes
                // Only proceed if the death animation has been played
                if (_hasPlayedDeathAnimation)
                {
                    // Destroy the enemy object after the death animation is complete
                    Object.Destroy(enemy.gameObject);  // You can add additional logic here before destroying the object, e.g., particle effects
                }
            }
        }
    }
}
