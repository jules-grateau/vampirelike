﻿using System.Collections;
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
            unityEvent.Invoke();
        }
    }

    public class GameEventListener<T, E, UER> : MonoBehaviour , IGameEventListener<T> where E: GameEvent<T> where UER: UnityEvent<T>
    {
        public E GameEvent 
        { 
            get => gameEvent;
            set { gameEvent = value; }
        }
        [SerializeField]
        private E gameEvent;

        public UER UnityEvent
        {
            get => unityEvent;
            set { unityEvent = value; }
        }
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

    public class GameEventListener<T,U, E, UER> : MonoBehaviour, IGameEventListener<T,U> where E : GameEvent<T,U> where UER : UnityEvent<T,U>
    {
        public E GameEvent
        {
            get => gameEvent;
            set { gameEvent = value; }
        }
        [SerializeField]
        private E gameEvent;

        public UER UnityEvent
        {
            get => unityEvent;
            set { unityEvent = value; }
        }
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

        public void RaiseEvent(T param, U param2)
        {
            unityEvent.Invoke(param, param2);
        }
    }
}