using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Controller.Ui
{
    public class UISelectedObjectOnAwake : MonoBehaviour
    {
        [SerializeField]
        GameObject _firstSelect;
        GameObject _previouslySelected;

        private void Awake()
        {
            _previouslySelected = EventSystem.current.currentSelectedGameObject;

            if (_firstSelect) EventSystem.current.SetSelectedGameObject(_firstSelect);
        }

        private void OnDestroy()
        {
            if(_previouslySelected) EventSystem.current.SetSelectedGameObject(_previouslySelected);
        }
    }
}