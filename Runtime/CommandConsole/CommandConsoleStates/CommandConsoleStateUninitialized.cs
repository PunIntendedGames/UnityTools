namespace PunIntended.Tools
{
    internal class CommandConsoleStateUninitialized : State<CommandConsole>
    {
        public override void OnEnter()
        {
            Owner.OnToggleConsole += OnOpenedConsole;
        }

        public override void OnExit()
        {
            Owner.OnToggleConsole -= OnOpenedConsole;
        }

        private void OnOpenedConsole() 
        {
            StateMachine.Switch<CommandConsoleStateInitialize>();
        }
    }
}