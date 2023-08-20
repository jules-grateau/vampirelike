using Assets.Types;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

namespace Assets.Scripts.Controller.Ui
{
    public class ActivableUI : MonoBehaviour
    {
        [SerializeField]
        InputActionReference _activationAction;
        [SerializeField]
        UnityEvent _OnDesactive;

        private void OnEnable()
        {
            _activationAction.action.performed += Desactive;
        }

        private void OnDisable()
        {
            _activationAction.action.performed -= Desactive;
        }

        void Desactive(CallbackContext context)
        {
            gameObject.SetActive(false);
            _OnDesactive.Invoke();
        }
        public void Activate()
        {
            gameObject.SetActive(true);
        }
    }
}