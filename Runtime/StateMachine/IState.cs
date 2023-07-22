namespace PunIntended.Tools
{
    public interface IState<TOwner, TState> where TState : IState<TOwner, TState>
    {
        public TOwner Owner { get; set; }
        public StateMachineAbstract<TOwner, TState> StateMachine { get; set; }
        public abstract void OnEnter();
        public abstract void OnExit();
    }
}