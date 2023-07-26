using Assets.Scripts.ScriptableObjects.Game;
using Assets.Scripts.ScriptableObjects.Items;
using Assets.Scripts.ScriptableObjects.Stage;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.Controller.Ui.CharacterSelection
{
    public class StageInfoController : MonoBehaviour
    {
        TextMeshProUGUI _name;
        Image _image;
        TextMeshProUGUI _description;
        string _sceneName;

        public void Init(StageSO stage)
        {
            _name = transform.Find("Name").GetComponent<TextMeshProUGUI>();
            _image = transform.Find("Image").GetComponent<Image>();
            _description = transform.Find("Description").GetComponent<TextMeshProUGUI>();
            _sceneName = stage.SceneName;

            _name.text = stage.Name;
            _image.sprite = stage.Sprite;
            _description.text = stage.Description;
        }

        public void OnClick()
        {
            SceneManager.LoadScene(_sceneName, LoadSceneMode.Single);
        }

    }
}