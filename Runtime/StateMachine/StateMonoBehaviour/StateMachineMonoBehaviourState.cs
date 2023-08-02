using System.Collections;
using UnityEngine;

namespace PunIntended.Tools
{
    public class StateMachineMonoBehaviourState<TOwner> : StateMachineAbstract<TOwner, StateMonoBehaviour<TOwner>> where TOwner : MonoBehaviour
    {
        public StateMachineMonoBehaviourState(TOwner owner, params StateMonoBehaviour<TOwner>[] states) : base(owner, states) { }

        public void Update()
        {
            foreach (StateMonoBehaviour<TOwner> state in CurrentStates)
            {
                state.OnUpdate();
            }
        }

        public void LateUpdate()
        {
            foreach (StateMonoBehaviour<TOwner> state in CurrentStates)
            {
                state.OnLateUpdate();
            }
        }

        public void FixedUpdate()
        {
            foreach (StateMonoBehaviour<TOwner> state in CurrentStates)
            {
                state.OnFixedUpdate();
            }
        }

        public void UpdateGUI()
        {
            foreach (StateMonoBehaviour<TOwner> state in CurrentStates)
            {
                state.OnGUIUpdate();
            }
        }
    }
}