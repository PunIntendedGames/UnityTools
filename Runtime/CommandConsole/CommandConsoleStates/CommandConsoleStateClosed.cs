using System.Collections;
using UnityEngine;

namespace PunIntended.Tools
{
    internal class CommandConsoleStateClosed : State<CommandConsole>
    {
        public override void OnEnter()
        {
            Owner.OnToggleConsole += OnOpen;
        }

        public override void OnExit()
        {
            Owner.OnToggleConsole -= OnOpen;
        }

        private void OnOpen()
        {
            StateMachine.Switch<CommandConsoleStateOpened>();
        }
    }
}