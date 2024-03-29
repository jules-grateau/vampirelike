﻿using Assets.Types;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.OnScreen;

namespace Assets.Scripts.Controller.Ui
{
    public class InputActionOnScreenButton : OnScreenButton, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            SendValueToControl(1.0f);
        }

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
            m_ControlPath = _action.action.bindings[0].effectivePath;
        }
    }
}