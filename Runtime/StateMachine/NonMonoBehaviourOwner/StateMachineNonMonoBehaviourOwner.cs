using System.Collections;
using UnityEngine;

namespace PunIntended.Tools
{
    public class StateMachineNonMonoBehaviourOwner<TOwner> : StateMachineAbstract<TOwner, StateNonMonoBehaviourOwner<TOwner>> where TOwner : class
    {
        public StateMachineNonMonoBehaviourOwner(TOwner owner, params StateNonMonoBehaviourOwner<TOwner>[] states) : base(owner, states) { }
    }
}