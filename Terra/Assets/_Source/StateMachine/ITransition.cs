namespace _Source.StateMachine {
    public interface ITransition {
        IState To { get; }
        IPredicate Condition { get; }
    }
}