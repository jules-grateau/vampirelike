using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField]
    protected string _sceneName;
    [SerializeField]
    LoadSceneMode loadSceneMode = LoadSceneMode.Single;
    [SerializeField]
    UnityEvent _onLoadScene;
    [SerializeField]
    UnityEvent _onUnloadScene;
    [SerializeField]
    string _loadingSceneName = "LoadingScene";

    private void Awake()
    {
        SceneManager.sceneUnloaded += OnUnloadScene;
    }

    public void LoadScene()
    {
        if (loadSceneMode == LoadSceneMode.Single)
        {
            SceneManager.LoadSceneAsync(_loadingSceneName, loadSceneMode).completed += LoadMainScene;
        }
        else
        {
            SceneManager.LoadScene(_sceneName, loadSceneMode);
        }
        _onLoadScene.Invoke();
    }

    void LoadMainScene(AsyncOperation operation)
    {
        SceneManager.LoadSceneAsync(_sceneName, loadSceneMode);
    }

    void OnUnloadScene(Scene current)
    {
        if (current.name != _sceneName) return;
        
        _onUnloadScene.Invoke();
    }
}
