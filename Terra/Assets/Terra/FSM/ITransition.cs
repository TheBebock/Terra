namespace Terra.FSM {
    public interface ITransition {
        IState To { get; }
        IPredicate Condition { get; }
    }
}