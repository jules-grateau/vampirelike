using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField]
    string _sceneName;
    [SerializeField]
    LoadSceneMode loadSceneMode = LoadSceneMode.Single;
    [SerializeField]
    UnityEvent _onLoadScene;
    [SerializeField]
    UnityEvent _onUnloadScene;

    private void Awake()
    {
        SceneManager.sceneUnloaded += OnUnloadScene;
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(_sceneName, loadSceneMode);
        _onLoadScene.Invoke();
    }

    void OnUnloadScene(Scene current)
    {
        if (current.name != _sceneName) return;
        
        _onUnloadScene.Invoke();
    }
}
