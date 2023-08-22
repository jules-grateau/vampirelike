using Assets.Scripts.Events.TypedEvents;
using Assets.Scripts.Types;
using UnityEngine;
using UnityEngine.EventSystems;

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
        GameObject _lastSelected;

        bool _isOpened = false;
        bool _isProcessing = false;
        private void Update()
        {
            if (!_isProcessing && _isOpened) _lastSelected = EventSystem.current.currentSelectedGameObject;
        }

        public void OnOpenInterfaceElement(InterfaceElement interfaceElement, bool isCloseAction)
        {
            _isProcessing = true;
            _isActive = interfaceElement == _interfaceElement;

            if (_isActive)
            {
                Open(isCloseAction);
            } else
            {
                Close();
            }

            _currActiveElement = interfaceElement;
            _isProcessing = false;
        }

        public void Open(bool isCloseAction)
        {
            if (_isOpened) return;

            transform.GetChild(0).gameObject.SetActive(true);
            if (_firstSelected) EventSystem.current.SetSelectedGameObject(_firstSelected);
            if (_lastSelected) EventSystem.current.SetSelectedGameObject(_lastSelected);
            if (!isCloseAction) _prevElement = _currActiveElement;
            _isOpened = true;
        }

        public void OpenPrevious()
        {
            _openInterfaceElementEvent.Raise(_prevElement, true);
        }

        public void Close()
        {
            if (!_isOpened) return;

            transform.GetChild(0).gameObject.SetActive(false);

            _isOpened = false;
        }
    }
}