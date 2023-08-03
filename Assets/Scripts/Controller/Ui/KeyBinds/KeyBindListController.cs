using Assets.Types;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using Utilities;

namespace Assets.Scripts.Controller.Ui.KeyBinds
{
    public class KeyBindListController : MonoBehaviour
    {

        PlayerInputs _playerInputs;

        GameObject _keybindActionPrefab;

        // Use this for initialization
        void Start()
        {
            _playerInputs = InputManager.GetInstance();
            _playerInputs.Disable();
            _keybindActionPrefab = Resources.Load<GameObject>("Prefabs/UI/KeyBindAction");


            foreach (InputAction action in _playerInputs)
            {
                GameObject instance = Instantiate(_keybindActionPrefab, transform.Find("View/Viewport/Content"));
                KeyBindActionController infoController = instance.AddComponent<KeyBindActionController>();
                infoController.Init(action);
            }
        }

        private void OnDestroy()
        {
            _playerInputs.Enable();
        }
    }
}