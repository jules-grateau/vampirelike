using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Events
{
    public interface IGameEventListener<T>
    {
        void RaiseEvent(T param);
    }

    public interface IGameEventListener<T, U>
    {
        void RaiseEvent(T param, U param2);
    }
}