using Assets.Scripts.Variables;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Controller.Ui.Stats
{
    public class StatInfoController : MonoBehaviour
    {
        [SerializeField]
        string _name;
        [SerializeField]
        string _valueAppendice;
        [SerializeField]
        FloatVariable _value;

        void Awake()
        {
            TextMeshProUGUI tmp = transform.Find("Text").GetComponent<TextMeshProUGUI>();
            tmp.text = $"{_name} : {_value.value.ToString()+_valueAppendice}";
        }
    }
}