using Assets.Scripts.Events;
using Assets.Scripts.Variables;
using Assets.Scripts.Variables.Constants;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Controller.Player
{
    public class PlayerHealth : MonoBehaviour
    {
        [SerializeField]
        private bool resetOnStart;
        [SerializeField]
        private FloatVariable hp;
        [SerializeField]
        private FloatVariable maxHp;

        private void OnEnable()
        {
            if (resetOnStart) hp.value = maxHp.value;
        }

        public void TakeDamage(float damage)
        {
            hp.value -= damage;
        }
    }
}