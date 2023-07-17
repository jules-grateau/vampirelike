using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Events
{
    [CreateAssetMenu(fileName = "GameEvent with no", menuName = "GameEvent/Default", order = 1)]
    public class GameEvent : ScriptableObject
    {
        private List<GameEventListener> gameEventListeners = new List<GameEventListener>();

        public void Raise()
        {
            for(int i = gameEventListeners.Count-1; i >= 0; i--)
            {
                gameEventListeners[i].RaiseEvent();
            }
        }

        public void RegisterListener(GameEventListener gameEventListener)
        {
            if (!gameEventListeners.Contains(gameEventListener))
                gameEventListeners.Add(gameEventListener);
        }

        public void UnregisterListener(GameEventListener gameEventListener)
        {
            if (gameEventListeners.Contains(gameEventListener))
                gameEventListeners.Remove(gameEventListener);
        }
    }

    public abstract class GameEvent<T> : ScriptableObject
    {
        private List<IGameEventListener<T>> gameEventListeners = new List<IGameEventListener<T>>();

        public void Raise(T param)
        {
            for (int i = gameEventListeners.Count - 1; i >= 0; i--)
            {
                gameEventListeners[i].RaiseEvent(param);
            }
        }

        public void RegisterListener(IGameEventListener<T> gameEventListener)
        {
            if (!gameEventListeners.Contains(gameEventListener))
                gameEventListeners.Add(gameEventListener);
        }

        public void UnregisterListener(IGameEventListener<T> gameEventListener)
        {
            if (gameEventListeners.Contains(gameEventListener))
                gameEventListeners.Remove(gameEventListener);
        }
    }
}