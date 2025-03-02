using System;
using System.Collections.Generic;

namespace Terra.StateMachine
{
    public class StateMachine
    {
         StateNode _current;
         Dictionary<Type, StateNode> _nodes = new();
         HashSet<ITransition> _anyTransitions = new();

         public void Update()
         {
             ITransition transiton = GetTransition();
             if (transiton != null)
             {
                 ChangeState(transiton.TargetState);
             }
             _current.State?.Update();
         }

         public void FixedUpdate()
         {
             _current.State?.FixedUpdate();
         }

         public void SetState(IState state)
         {
             _current = _nodes[state.GetType()];
             _current.State?.OnEnter();
         }

         void ChangeState(IState state)
         {
             if (state == _current.State)
             {
                 return;
             }
             var previousState = _current.State;
             var nextState = _nodes[state.GetType()].State;
             
             previousState?.OnExit();
             nextState?.OnEnter();
             _current = _nodes[state.GetType()];  
         }

         ITransition GetTransition()
         {
             foreach (var transition in _anyTransitions)
             {
                 if(transition.Condition.Evaluate())
                     return transition;
             }

             foreach (var transition in _current.Transitions)
             {
                 if(transition.Condition.Evaluate())
                     return transition;
             }
             return null;
         }

         public void AddTransition(IState from, IState targetState, IPredicate predicate)
         {
             GetOrAddNode(from).AddTransition(GetOrAddNode(targetState).State, predicate);
         }

         public void AddAnyTransition(IState targetState, IPredicate predicate)
         {
             _anyTransitions.Add(new Transition(GetOrAddNode(targetState).State, predicate));
         }

         StateNode GetOrAddNode(IState state)
         {
             var node = _nodes.GetValueOrDefault(state.GetType());
             if (node == null)
             {
                 node = new StateNode(state);
                 _nodes.Add(state.GetType(), node);
             }
             return node;
         }


         class StateNode
         {
            public IState State { get; }
            public HashSet<ITransition> Transitions { get; }

            public StateNode(IState state)
            {
                State = state;
                Transitions = new HashSet<ITransition>();
            }

            public void AddTransition(IState targetstate, IPredicate predicate)
            {
                Transitions.Add(new Transition(targetstate, predicate));
            }
         }
    }
}