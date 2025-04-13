using Terra.AI.States.EnemyStates;
using UnityEngine;

namespace Terra.AI.States.EnemyStates
{

    public class EnemyDeathState : EnemyBaseState
    {
        public EnemyDeathState(Enemy enemy, Animator animator) : base(enemy, animator)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            animator.CrossFade(DieHash, crossFadeDuration);
        }
    }
}