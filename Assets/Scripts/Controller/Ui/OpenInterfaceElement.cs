using Assets.Scripts.Events.TypedEvents;
using Assets.Scripts.Types;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Controller.Ui
{
    public class OpenInterfaceElement : MonoBehaviour
    {
        [SerializeField]
        InterfaceElement _interfaceElementToOpen;
        [SerializeField]
        GameEventInterfaceElement _gameEventInterfaceElement;

        public void OnOpenInterface()
        {
            _gameEventInterfaceElement.Raise(_interfaceElementToOpen, false);
        }

    }
}