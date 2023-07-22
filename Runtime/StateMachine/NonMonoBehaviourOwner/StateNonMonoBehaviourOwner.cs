namespace PunIntended.Tools
{
    public class StateNonMonoBehaviourOwner<TOwner> : IState<TOwner, StateNonMonoBehaviourOwner<TOwner>> where TOwner : class
    {
        public TOwner Owner { get; set; }
        public StateMachineAbstract<TOwner, StateNonMonoBehaviourOwner<TOwner>> StateMachine { get; set; }
        public virtual void OnEnter() { }
        public virtual void OnExit() { }
    }
}