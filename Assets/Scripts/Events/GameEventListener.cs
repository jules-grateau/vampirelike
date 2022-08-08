using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Events
{
    public class GameEventListener : MonoBehaviour
    {
        [SerializeField]
        private GameEvent gameEvent;
        [SerializeField]
        private UnityEvent unityEvent;

        private void OnEnable()
        {
            gameEvent.RegisterListener(this);
        }

        private void OnDisable()
        {
            gameEvent.UnregisterListener(this);
        }

        public void RaiseEvent()
        {
            Debug.Log("RaiseEvent");
            unityEvent.Invoke();
        }
    }

    public class GameEventListener<T, E, UER> : MonoBehaviour , IGameEventListener<T> where E: GameEvent<T> where UER: UnityEvent<T>
    {
        [SerializeField]
        private E gameEvent;
        [SerializeField]
        private UER unityEvent;

        private void OnEnable()
        {
            gameEvent.RegisterListener(this);
        }

        private void OnDisable()
        {
            gameEvent.UnregisterListener(this);
        }

        public void RaiseEvent(T param)
        {
            unityEvent.Invoke(param);
        }
    }
}