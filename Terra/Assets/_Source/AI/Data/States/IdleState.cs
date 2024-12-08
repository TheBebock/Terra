using System.Collections;
using System.Collections.Generic;
using AI;
using AI.Data.States;
using UnityEngine;

public class IdleState : AIState
{
    public override void StartState(AIController controller)
    {
        Debug.Log("Entering idle state");

        controller.GetComponent<Animator>()?.Play("Idle");
    }

    public override void UpdateState(AIController controller)
    {
        float detectionRange = 10f;
        Collider[] hits = Physics.OverlapSphere(controller.transform.position, detectionRange);

        for (int i = 0; i < hits.Length; i++)
        {
            Collider hit = hits[i];
            if (hit.CompareTag("Player"))
            {
                Debug.Log("Player detected, transition");
                break;
            }
        }

        throw new System.NotImplementedException();
    }

    public override void ExitState(AIController controller)
    {
        Debug.Log("Exiting idle state");
        Animator animator = controller.GetComponent<Animator>();
        if (animator != null && animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            animator.Play("Empty");
        }
    }

    public override bool CanExitState(AIController controller)
    {
        float detectionRange = 10f;
        Collider[] hits = Physics.OverlapSphere(controller.transform.position, detectionRange);

        for (int i = 0; i < hits.Length; i++)
        {
            Collider hit = hits[i];
            if (hit.CompareTag("Player"))
            {
                Debug.Log("Player detected, can exit IdleState.");
                return true;
            }
        }

        Debug.Log("No player detected, cannot exit IdleState.");
        return false;
    }

    public override bool CanEnterState(AIController controller)
    {


        //    if (nie ma spalnionych warunkow do wejscia)
        {
            //      return false;
        }

        //sa warunki do wejscia
        return true;

    }
}
