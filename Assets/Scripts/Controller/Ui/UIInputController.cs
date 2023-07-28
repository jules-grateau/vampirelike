using Assets.Scripts.Events;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Controller.Ui
{
    public class UIInputController : MonoBehaviour
    {
        bool _isPauseMenuOpen = false;
        [SerializeField]
        GameEvent _pauseEvent;
        [SerializeField]
        GameEvent _unpauseEvent;

        bool _isPaused = false;
        bool _isPlayerDead = false;

        void Update()
        {
          if (_isPlayerDead) return;

          if(Input.GetKeyDown(KeyCode.Escape))
            {
                if(_isPauseMenuOpen && _isPaused)
                {
                    ClosePauseMenu();
                    return;
                }
                if(!_isPauseMenuOpen && !_isPaused)
                {
                    OpenPauseMenu();
                }
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