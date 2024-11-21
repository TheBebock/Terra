using System.Collections;
using System.Collections.Generic;
using AI.Data.States;
using UnityEngine;

namespace AI
{
    public abstract class AIController : MonoBehaviour
    {
        [SerializeField] AIState[] AIStates;
        AIState _previous;
        AIState _current;
        AIState _next;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }


}