using Assets.Scripts.Controller.Game;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Controller.Ui
{
    public class DisplayWeaponDamage : MonoBehaviour
    {
        [SerializeField]
        GameObject _targetPanel;

        [SerializeField]
        GameObject _prefab;
        // Use this for initialization
        void Start()
        {
            if (!_targetPanel) _targetPanel = this.gameObject;
            
            if(!_prefab) _prefab = Resources.Load<GameObject>("Prefabs/UI/WeaponDamageInfo");

            List<KeyValuePair<string, float>> damageDone = GameStatistics.Instance.GetWeaponDamagesList();

            foreach(KeyValuePair<string,float> weaponDamage in damageDone)
            {
                GameObject weaponLine = Instantiate(_prefab, _targetPanel.transform);
                TextMeshProUGUI text = weaponLine.transform.Find("Text").GetComponent<TextMeshProUGUI>();

                text.SetText($"{weaponDamage.Key} : {weaponDamage.Value}");
            }
        }
    }
}