using UnityEngine.UIElements;

namespace PunIntended.Tools
{
    internal class CommandConsoleStateOpened : State<CommandConsole>
    {
        public override void OnEnter()
        {
            Owner.OnToggleConsole += OnClose;
        }

        public override void OnExit()
        {
            Owner.OnToggleConsole -= OnClose;
        }

        private void OnClose()
        {
            StateMachine.Switch<CommandConsoleStateClosed>();
        }
    }
}