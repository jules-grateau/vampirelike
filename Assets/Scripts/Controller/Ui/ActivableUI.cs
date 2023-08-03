using Assets.Types;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

namespace Assets.Scripts.Controller.Ui
{
    public class ActivableUI : MonoBehaviour
    {
        PlayerInputs _playerInput;

        private void OnEnable()
        {
            _playerInput = InputManager.GetInstance();
            _playerInput.Gameplay.Pause.performed += Desactive;
        }

        private void OnDisable()
        {
            _playerInput.Gameplay.Pause.performed -= Desactive;
        }

        void Desactive(CallbackContext context)
        {
            gameObject.SetActive(false);
        }
        public void Activate()
        {
            gameObject.SetActive(true);
        }
    }
}