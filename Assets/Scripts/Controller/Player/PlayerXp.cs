using Assets.Scripts.Controller.Game;
using Assets.Scripts.Events;
using Assets.Scripts.Variables;
using UnityEngine;

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
        private GameObject _LevelUpEffectGO;
        [SerializeField]
        private AudioClip _LevelUpAudioClip;

        private ParticleSystem _LevelUpEffect;

        [SerializeField]
        private GameEvent _playerLevelUpEvent;

        void Awake()
        {
            _LevelUpEffect = Instantiate(_LevelUpEffectGO.GetComponent<ParticleSystem>(), gameObject.transform.position, Quaternion.identity, gameObject.transform);
        }

        private void Start()
        {
            _currentLevel.value = 1;
            _xp.value = 0;
            _maxXp.value = XpToReach(_currentLevel);
        }

        public void OnPlayerGainXp(float value)
        {
            _xp.value += value;
            int xpToReach = XpToReach(_currentLevel);

            while(_xp.value >= xpToReach)
            {
                AudioSource.PlayClipAtPoint(_LevelUpAudioClip, transform.position, 1);
                _LevelUpEffect.Play();
                _xp.value = _xp.value - xpToReach;
                _currentLevel.value += 1;
                _maxXp.value = XpToReach(_currentLevel);
                _playerLevelUpEvent.Raise();
                xpToReach = XpToReach(_currentLevel);
            }
        }

        private int XpToReach(IntVariable level)
        {
            return XpToReach((float)level.value);
        }


        private int XpToReach(float level)
        {
            return Mathf.RoundToInt(GameManager.GameState.XpCurve.Evaluate(level));
        }
    }
}