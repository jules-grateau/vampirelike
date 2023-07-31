using Assets.Scripts.ScriptableObjects.Characters;
using Assets.Scripts.Variables;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Controller.Ui.Stats
{
    public class StatInfoController : MonoBehaviour
    {
        public void Init(string name, float value, string valueAppendix)
        {
            TextMeshProUGUI tmp = transform.Find("Text").GetComponent<TextMeshProUGUI>();
            tmp.text = $"{name} : {value.ToString()+valueAppendix}";
        }
    }
}