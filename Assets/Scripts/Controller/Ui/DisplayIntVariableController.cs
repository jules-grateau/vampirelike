using Assets.Scripts.Variables;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Controller.Ui
{
    public class DisplayIntVariableController : MonoBehaviour
    {
        [SerializeField]
        IntVariable _variable;
        [SerializeField]
        TextMeshProUGUI _text;

        private void Update()
        {
            _text.SetText(_variable.value.ToString());
        }

    }
}