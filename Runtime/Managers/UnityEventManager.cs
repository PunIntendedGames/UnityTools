using System;
using System.Collections.Generic;
using UnityEngine;

namespace PunIntended.Tools
{
    public class UnityEventManager : LazyMonoBehaviourSingleton<UnityEventManager>
    {
        private readonly Dictionary<UpdateMethodType, Dictionary<Action, MonoBehaviour>> _subscribers = new();

        private void Update() => InvokeUpdateMethodType(UpdateMethodType.Update);
        private void FixedUpdate() => InvokeUpdateMethodType(UpdateMethodType.FixedUpdate);
        private void LateUpdate() => InvokeUpdateMethodType(UpdateMethodType.LateUpdate);
        private void OnGUI() => InvokeUpdateMethodType(UpdateMethodType.GUIUpdate);

        private void InvokeUpdateMethodType(UpdateMethodType updateMethodType)
        {
            if (_subscribers.TryGetValue(updateMethodType, out Dictionary<Action, MonoBehaviour> subscribers))
            {
                // keep track of unsubscribers (subscribed methods whose owner has become null),
                // have to keep track in a seperate list to avoid modifying the collection during enumeration
                List<Action> unsubscribers = new();
                foreach (KeyValuePair<Action, MonoBehaviour> subscriber in subscribers)
                {
                    if (subscriber.Value != null)
                    {
                        if (subscriber.Value.isActiveAndEnabled)
                        {
                            subscriber.Key.Invoke(); // no null check on purpose, we want to know if this blows up!
                        }
                    }
                    else
                    {
                        unsubscribers.Add(subscriber.Key);
                    }
                }

                foreach (Action method in unsubscribers)
                {
                    subscribers.Remove(method);
                }
            }
        }

        public void Subscribe(Action method, UpdateMethodType updateMethodType, MonoBehaviour owner)
        {
            if (_subscribers.TryGetValue(updateMethodType, out Dictionary<Action, MonoBehaviour> methodTypeSubscribers))
            {
                if (!methodTypeSubscribers.TryAdd(method, owner))
                {
                    Debug.LogWarning($"{method} is already subscribed to {updateMethodType}!", owner);
                }
            }
            else
            {
                Dictionary<Action, MonoBehaviour> subscriberDictionary = new()
                {
                    {
                        method, owner
                    }
                };

                _subscribers.Add(updateMethodType, subscriberDictionary);
            }
        }

        public void Unsubscribe(Action method, UpdateMethodType updateMethodType, MonoBehaviour owner)
        {
            if (_subscribers.TryGetValue(updateMethodType, out Dictionary<Action, MonoBehaviour> subscribers))
            {
                if (subscribers.ContainsKey(method))
                {
                    subscribers.Remove(method);
                }
                else
                {
                    Debug.LogWarning($"{method} was not subscribed to {updateMethodType}!", owner);
                }
            }
            else
            {
                Debug.LogWarning($"no methods have been subscribed to {updateMethodType}!");
            }
        }

        public enum UpdateMethodType
        {
            Update,
            FixedUpdate,
            LateUpdate,
            GUIUpdate
        }
    }
}