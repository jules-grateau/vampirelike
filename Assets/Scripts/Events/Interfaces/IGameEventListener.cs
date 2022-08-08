using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Events
{
    public interface IGameEventListener<T>
    {
        void RaiseEvent(T param);
    }
}