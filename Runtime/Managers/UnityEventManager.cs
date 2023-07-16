using System;

namespace PunIntended.Tools
{
    public class UnityEventManager : LazyMonoBehaviourSingleton<UnityEventManager>
    {
        public event Action OnUpdate;
        public event Action OnLateUpdate;
        public event Action OnFixedUpdate;
        public event Action OnGuiUpdate;

        private void Update()
        {
            OnUpdate?.Invoke();
        }

        private void LateUpdate()
        {
            OnLateUpdate?.Invoke();
        }

        private void FixedUpdate() 
        {
            OnFixedUpdate?.Invoke();
        }

        private void OnGUI()
        {
            OnGuiUpdate?.Invoke();
        }
    }
}