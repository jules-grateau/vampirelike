using Assets.Scripts.Events.TypedEvents;
using Assets.Scripts.Types;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

namespace Assets.Scripts.Controller.Ui
{
    public class UIWrapperController : MonoBehaviour
    {
        [SerializeField]
        InterfaceElement _interfaceElement;
        bool _isActive = false;

        InterfaceElement _currActiveElement = InterfaceElement.Menu;
        InterfaceElement _prevElement = InterfaceElement.Menu;

        [SerializeField]
        GameEventInterfaceElement _openInterfaceElementEvent;

        [SerializeField]
        GameObject _firstSelected;

        public void OnOpenInterfaceElement(InterfaceElement interfaceElement, bool isCloseAction)
        {
            _isActive = interfaceElement == _interfaceElement;
            transform.GetChild(0).gameObject.SetActive(_isActive);

            if (_isActive)
            {
                if(_firstSelected) EventSystem.current.SetSelectedGameObject(_firstSelected);
                if(!isCloseAction) _prevElement = _currActiveElement;
            }

            _currActiveElement = interfaceElement;
        }

        public void Close()
        {
            _openInterfaceElementEvent.Raise(_prevElement, true);
        }
    }
}