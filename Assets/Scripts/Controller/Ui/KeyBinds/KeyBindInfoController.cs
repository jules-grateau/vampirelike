using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputControlPath;

namespace Assets.Scripts.Controller.Ui.KeyBinds
{
    public class KeyBindInfoController : MonoBehaviour
    {
        InputBinding _inputBinding;
        InputAction _inputAction;
        TextMeshProUGUI _buttonText;
        Button _button;
        int _bindingIndex;
        public void Init(InputBinding binding, InputAction action, bool isComposite)
        {
            _inputBinding = binding;
            _inputAction = action;
            _bindingIndex = _inputAction.GetBindingIndex(_inputBinding);

            TextMeshProUGUI text = transform.Find("Description").GetComponent<TextMeshProUGUI>();
            _button = transform.Find("Button").GetComponent<Button>();
            _buttonText = _button.transform.Find("Text").GetComponent<TextMeshProUGUI>();
            text.SetText(isComposite ? binding.name : binding.action);

            _buttonText.SetText(InputControlPath.ToHumanReadableString(binding.effectivePath, HumanReadableStringOptions.OmitDevice));
            _button.onClick.AddListener(OnClick);
        }

        void OnClick()
        {
            _inputAction.PerformInteractiveRebinding(_bindingIndex)
                .WithControlsExcluding("Mouse")
                .Start()
                .OnComplete((callback) => {
                    _buttonText.SetText(InputControlPath.ToHumanReadableString(callback.action.bindings[_bindingIndex].effectivePath, HumanReadableStringOptions.OmitDevice));
                    _button.interactable = true;
                    SetSelected();
                    callback.Dispose();
                    });
            _buttonText.SetText("Waiting for input ...");
            _button.interactable = false;
        }

        public void SetSelected()
        {
            EventSystem.current.SetSelectedGameObject(_button.gameObject);
        }


    }
}