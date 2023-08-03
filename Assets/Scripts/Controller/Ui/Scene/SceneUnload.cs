using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Controller.Ui.Scene
{
    public class SceneUnload : MonoBehaviour
    {
        public void UnloadScene()
        {
            SceneManager.UnloadSceneAsync(gameObject.scene);
        }
    }
}