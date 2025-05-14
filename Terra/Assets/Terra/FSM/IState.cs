namespace Terra.FSM {
    public interface IState {
        void OnEnter();
        void Update();
        void OnExit();
    }
}