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

        void Update()
        {
          if(Input.GetKeyDown(KeyCode.Escape))
            {
                if(_isPauseMenuOpen)
                {
                    ClosePauseMenu();
                    return;
                }
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


    }
}