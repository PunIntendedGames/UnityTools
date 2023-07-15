using UnityEngine;

namespace PunIntended.Tools
{
    public class StateMachineRuntime<TOwner> : StateMachine<TOwner, StateRuntime<TOwner>> where TOwner : MonoBehaviour
    {
        public StateMachineRuntime(TOwner owner, params StateRuntime<TOwner>[] states) : base(owner, states) { }

        public void Update()
        {
            foreach (StateRuntime<TOwner> state in CurrentStates)
            {
                state.OnUpdate();
            }
        }

        public void LateUpdate()
        {
            foreach (StateRuntime<TOwner> state in CurrentStates)
            {
                state.OnUpdate();
            }
        }

        public void FixedUpdate()
        {
            foreach (StateRuntime<TOwner> state in CurrentStates)
            {
                state.OnFixedUpdate();
            }
        }
    }
}