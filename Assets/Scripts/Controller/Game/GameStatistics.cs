using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Controller.Game
{
    public class GameStatistics : MonoBehaviour
    {
        public static GameStatistics Instance => _instance;
        private static GameStatistics _instance;

        public int EnemiesKilled { get; private set; }
        private Dictionary<string,int> _enemiesKilledDic;
        private Dictionary<string, float> _weaponDamageDic;

        private void Awake()
        {
            if (_instance != null) Debug.LogError("More than one instance of GameStatistics exist. It should be a Singleton");

            _instance = this;
            EnemiesKilled = 0;
            _enemiesKilledDic = new Dictionary<string, int>();
            _weaponDamageDic = new Dictionary<string, float>();
        }

        public void OnEnemyDie(string name)
        {
            EnemiesKilled++;
            if(_enemiesKilledDic.ContainsKey(name))
            {
                _enemiesKilledDic[name]++;
            } else
            {
                _enemiesKilledDic.Add(name, 1);
            }
        }

        public void OnEnemyLogDamage(HitData hit)
        {
            if (hit.weapon == null) return;
            string name = hit.weapon.Name;
            if (_weaponDamageDic.ContainsKey(name))
            {
                _weaponDamageDic[name] += hit.damage;
            } else
            {
                _weaponDamageDic.Add(name, hit.damage);
            }
        }

        public List<KeyValuePair<string,int>> GetEnemiesKilledList()
        {
            return _enemiesKilledDic.ToList();
        }

        public List<KeyValuePair<string,float>> GetWeaponDamagesList()
        {
            return _weaponDamageDic.ToList();
        }
    }
}