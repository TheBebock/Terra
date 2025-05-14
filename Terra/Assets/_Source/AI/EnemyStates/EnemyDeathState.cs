using _Source.AI.Enemy;
using Terra.AI.Enemies;
using UnityEngine;
using UnityEngine.AI;

namespace _Source.AI.EnemyStates
{
    public class EnemyDeathState : EnemyBaseState
    {
        // Statyczny hash animacji śmierci, żeby uniknąć konwersji w każdym wywołaniu
        public static readonly int DieHash = Animator.StringToHash("Die");

        public EnemyDeathState(EnemyBase enemy, NavMeshAgent agent, Animator animator) : base(enemy, agent, animator)
        {
        }

        public override void OnEnter()
        {
            // Sprawdzenie, czy animacja nie jest już uruchomiona
            if (Animator.GetCurrentAnimatorStateInfo(0).IsName("Die")) return;

            // Uruchomienie animacji śmierci z płynnością
            base.OnEnter();
            Animator.CrossFade(DieHash, CrossFadeDuration);
        }

        public override void Update()
        {
            // Jeżeli animacja śmierci jest zakończona
            if (Animator.GetCurrentAnimatorStateInfo(0).IsName("Die") && Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                // Logika po zakończeniu animacji, np. usunięcie wroga
                //Destroy(Enemy.gameObject);
            }
        }

    }
}