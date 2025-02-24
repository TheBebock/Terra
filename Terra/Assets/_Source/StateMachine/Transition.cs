namespace _Source.StateMachine
{
    public class Transition : ITransition
    {
        public IState TargetState { get;}
        public IPredicate Predicate { get; }

        public Transition(IState targetState, IPredicate predicate)
        {
            TargetState = targetState;
            Predicate = predicate;
        }
    }
}