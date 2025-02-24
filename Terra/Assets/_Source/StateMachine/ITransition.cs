namespace _Source.StateMachine
{
    public interface ITransition
    {
        IState TargetState { get; }
        IPredicate Predicate { get; }
    }
}