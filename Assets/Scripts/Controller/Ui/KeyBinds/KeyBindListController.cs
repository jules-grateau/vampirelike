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
        [SerializeField]
        InputActionReference[] _bindableActions;

        [SerializeField]
        string _bindingGroups;

        GameObject _keybindActionPrefab;

        PlayerInputs _playerInput;

        // Use this for initialization
        void Start()
        {
            _playerInput = InputManager.GetInstance();
            _playerInput.Disable();

            _keybindActionPrefab = Resources.Load<GameObject>("Prefabs/UI/KeyBindAction");

            //We match the InputActionReference with the actual input references
            InputAction[] _playerActions = _playerInput.Where(action => _bindableActions.FirstOrDefault(bindableAction => bindableAction.action.id == action.id) != null).ToArray();

            for(int i = 0; i < _playerActions.Length; i++)
            {
                InputAction action = _playerActions[i];
                _playerInput.FindAction(action.name);
                GameObject instance = Instantiate(_keybindActionPrefab, transform.Find("View/Viewport/Content"));
                KeyBindActionController infoController = instance.AddComponent<KeyBindActionController>();
                infoController.Init(action, _bindingGroups, i==0);
            }
        }

        private void OnDestroy()
        {
            _playerInput.Enable();
        }
    }
}