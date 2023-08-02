using System;
using UnityEngine;

namespace PunIntended.Tools
{
    public class StateMachine<TOwner> : StateMachineAbstract<TOwner, State<TOwner>> where TOwner : MonoBehaviour
    {
        public StateMachine(TOwner owner, params State<TOwner>[] states) : base(owner, states) { }

        public void Update()
        {
            foreach (State<TOwner> state in CurrentStates)
            {
                state.OnUpdate();
            }
        }

        public void LateUpdate()
        {
            foreach (State<TOwner> state in CurrentStates)
            {
                state.OnLateUpdate();
            }
        }

        public void FixedUpdate()
        {
            foreach (State<TOwner> state in CurrentStates)
            {
                state.OnFixedUpdate();
            }
        }

        public void GUIUpdate()
        {
            foreach (State<TOwner> state in CurrentStates)
            {
                state.OnGUIUpdate();
            }
        }
    }
}