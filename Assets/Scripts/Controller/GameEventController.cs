using Assets.Scripts.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventController : MonoBehaviour
{
    [SerializeField]
    GameEvent _gameEvent;

    public void Raise()
    {
        _gameEvent.Raise();
    }
}
