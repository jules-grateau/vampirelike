using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Controller.Ui.KeyBinds
{
    public class KeyBindActionController : MonoBehaviour
    {
        GameObject _keybindInfoPrefab;

        public void Init(InputAction action, string bindingGroups, bool isFirst)
        {
            _keybindInfoPrefab = Resources.Load<GameObject>("Prefabs/UI/KeyBindInfo");
            GameObject mainSection = transform.Find("SectionContainer/Main").gameObject;
            GameObject secondarySection = transform.Find("SectionContainer/Secondary").gameObject;
            TextMeshProUGUI actionName = transform.Find("ActionName").GetComponent<TextMeshProUGUI>();

            bool isActionCompite = action.bindings[0].isComposite;
            GameObject parentSection = mainSection;

            if(isActionCompite)
            {
                actionName.SetText(action.bindings[0].action);
            } else
            {
                Destroy(actionName.gameObject);
                Destroy(mainSection.transform.Find("Title").gameObject);
                Destroy(secondarySection.transform.Find("Title").gameObject);
            }

            bool isFirstButtonSelected = false;

            for(int i = 0; i < action.bindings.Count; i++)
            {
                InputBinding binding = action.bindings[i];

                if (isActionCompite && binding.isComposite && binding.name == "Secondary") parentSection = secondarySection;
                if (binding.isComposite)
                {
                    TextMeshProUGUI title = parentSection.transform.Find("Title").GetComponent<TextMeshProUGUI>();
                    title.SetText(binding.name);
                    continue;
                };
                if (binding.groups != bindingGroups) continue;

                GameObject instance = Instantiate(_keybindInfoPrefab, parentSection.transform);
                KeyBindInfoController infoController = instance.AddComponent<KeyBindInfoController>();
                infoController.Init(binding, action, isActionCompite);

                if (isFirst && !isFirstButtonSelected)
                {
                    infoController.SetSelected();
                    isFirstButtonSelected = true;
                }
            }
        }
    }
}