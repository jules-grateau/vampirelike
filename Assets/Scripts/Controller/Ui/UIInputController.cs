using Assets.Scripts.Events;
using Assets.Types;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.InputSystem.InputAction;

namespace Assets.Scripts.Controller.Ui
{
    public class UIInputController : MonoBehaviour
    {
        PlayerInputs _playerInput;
        bool _isPauseMenuOpen = false;
        [SerializeField]
        GameEvent _pauseEvent;
        [SerializeField]
        GameEvent _unpauseEvent;

        bool _isPaused = false;
        bool _isPlayerDead = false;

        private void OnEnable()
        {
            _playerInput = InputManager.GetInstance();
            _playerInput.Gameplay.Pause.performed += HandlePause;
        }

        private void OnDisable()
        {
            _playerInput.Gameplay.Pause.performed -= HandlePause;
        }

        void HandlePause(CallbackContext context)
        {
            if (_isPauseMenuOpen && _isPaused)
            {
                ClosePauseMenu();
                return;
            }
            if (!_isPauseMenuOpen && !_isPaused)
            {
                OpenPauseMenu();
            }
        } 
        void OpenPauseMenu()
        {
            _pauseEvent.Raise();
            SceneManager.LoadScene("PauseMenu", LoadSceneMode.Additive);
            _isPauseMenuOpen = true;
        }

        void ClosePauseMenu()
        {
            SceneManager.UnloadSceneAsync("PauseMenu");
            _isPauseMenuOpen = false;
            _unpauseEvent.Raise();
        }

        public void OnPause()
        {
            _isPaused = true;
        }

        public void OnUnpause()
        {
            _isPaused = false;
        }

        public void OnPlayerDeath()
        {
            _isPlayerDead = true;
            Debug.Log(_isPlayerDead);
        }


    }
}