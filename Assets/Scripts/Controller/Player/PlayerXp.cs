using Assets.Scripts.Events;
using Assets.Scripts.Variables;
using Assets.Scripts.Variables.Constants;
using System.Collections;
using UnityEngine;
using Assets.Scripts.ScriptableObjects.Items;

namespace Assets.Scripts.Controller.Player
{
    public class PlayerXp : MonoBehaviour
    {
        [SerializeField]
        private FloatVariable _xp;
        [SerializeField]
        private FloatVariable _maxXp;

        private ParticleSystem _LevelUpEffect;

        [SerializeField]
        private GameEvent _playerLevelUpEvent;

        void Awake()
        {
            this._LevelUpEffect = Instantiate(Resources.Load<ParticleSystem>("Prefabs/Particles/glow_1"), gameObject.transform.position, Quaternion.identity, gameObject.transform);
        }

        private void OnEnable()
        {
            _xp.value = 0;
        }

        public void Collect(CollectibleSO collect)
        {
            _xp.value += ((XpCollectible)collect)._xpValue;

            if (_xp.value >= _maxXp.value)
            {
                this._LevelUpEffect.Play();
                _xp.value = _xp.value - _maxXp.value;
                _playerLevelUpEvent.Raise();
            }
        }
    }
}