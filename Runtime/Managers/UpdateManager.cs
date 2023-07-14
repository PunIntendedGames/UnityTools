using System;

namespace PunIntended.Tools
{
    public class UpdateManager : LazyMonoBehaviourSingleton<UpdateManager>
    {
        public event Action OnUpdate;
        public event Action OnFixedUpdate;
        public event Action OnLateUpdate;

        private void Update() 
        {
            OnUpdate?.Invoke();
        } 

        private void FixedUpdate()
        {
            OnFixedUpdate?.Invoke();
        }

        private void LateUpdate()
        {
            OnLateUpdate?.Invoke();
        }
    }
}