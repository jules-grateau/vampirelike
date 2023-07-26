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
        private bool _resetOnStart;
        [SerializeField]
        private FloatVariable _hp;
        [SerializeField]
        private FloatVariable _maxHp;
        [SerializeField]
        private GameEvent _onPlayerDeathEvent;

        private void OnEnable()
        {
            if (_resetOnStart) _hp.value = _maxHp.value;
        }

        public void TakeDamage(float damage)
        {
            _hp.value -= damage;
            if(_hp.value <= 0)
            {
                Die();
            } 
        }

        void Die()
        {
            Destroy(gameObject);
            _onPlayerDeathEvent.Raise();
        }
    }
}