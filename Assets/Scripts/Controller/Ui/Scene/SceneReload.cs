using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Controller.Ui.Scene
{
    public class SceneReload : MonoBehaviour
    {
        public void ReloadScene()
        {
            string activeSceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(activeSceneName, LoadSceneMode.Single);
        }
    }
}