using System.Collections;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Controller.Ui.Scene
{
    public class InitialGameLoad : MonoBehaviour
    {
        [SerializeField]
        string _startScene = "MainMenu";

        private void Start()
        {
            StartCoroutine(LoadAll());
        }

        IEnumerator LoadAll()
        {
            yield return LocalizationSettings.InitializationOperation;

            SceneManager.LoadSceneAsync(_startScene);
        }
    }
}