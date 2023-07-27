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
        [SerializeField]
        private IntVariable _currentLevel;
        [SerializeField]
        private AnimationCurve _xpCurve;

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
            _maxXp.value = XpToReach(_currentLevel);
        }

        public void Collect(CollectibleSO collect)
        {
            if (!(collect is XpCollectible)) return;

            _xp.value += ((XpCollectible)collect)._xpValue;
            int xpToReach = XpToReach(_currentLevel);

            if (_xp.value >= xpToReach)
            {
                this._LevelUpEffect.Play();
                _xp.value = _xp.value - xpToReach;
                _currentLevel.value += 1;
                _maxXp.value = XpToReach(_currentLevel);
                _playerLevelUpEvent.Raise();
            }
        }

        private int XpToReach(IntVariable level)
        {
            return XpToReach((float)level.value);
        }

        private int XpToReach(int level)
        {
            return Mathf.RoundToInt((float)level);
        }

        private int XpToReach(float level)
        {
            return Mathf.RoundToInt(_xpCurve.Evaluate(level));
        }
    }
}