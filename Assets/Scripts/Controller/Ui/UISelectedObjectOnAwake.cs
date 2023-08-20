using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Controller.Ui
{
    public class UISelectedObjectOnAwake : MonoBehaviour
    {
        [SerializeField]
        GameObject _firstSelect;

        private void Awake()
        {
            if(_firstSelect) EventSystem.current.SetSelectedGameObject(_firstSelect);
        }
    }
}