using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField]
    string _sceneName;
    [SerializeField]
    LoadSceneMode loadSceneMode = LoadSceneMode.Single;

    public void LoadScene()
    {
        SceneManager.LoadScene(_sceneName, loadSceneMode);
    }
}
