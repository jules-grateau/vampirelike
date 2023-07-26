using Assets.Scripts.ScriptableObjects.Characters;
using Assets.Scripts.Variables;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Controller.Ui.Stats
{
    public class StatInfoController : MonoBehaviour
    {
        public void Init(StatisticSO statistic, float value)
        {
            TextMeshProUGUI tmp = transform.Find("Text").GetComponent<TextMeshProUGUI>();
            tmp.text = $"{statistic.Name} : {value.ToString()+statistic.ValueAppendix}";
        }
    }
}