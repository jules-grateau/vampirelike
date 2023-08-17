using Assets.Types;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.OnScreen;

namespace Assets.Scripts.Controller.Ui
{
    public class InputActionOnScreenButton : OnScreenControl, IPointerDownHandler
    {
        public void OnPointerDown(PointerEventData eventData)
        {
            SendValueToControl(1.0f);
        }

        [InputControl(layout = "Button")]
        private string m_ControlPath;

        [SerializeField]
        private InputActionReference _action;

        protected override string controlPathInternal
        {
            get => m_ControlPath;
            set => m_ControlPath = value;
        }

        private void Awake()
        {
            this.controlPath = _action.action.bindings[0].effectivePath;
        }
    }
}