using Assets.Scripts.Controller.Game;
using Assets.Scripts.Events;
using Assets.Scripts.Variables;
using UnityEngine;
using Assets.Scripts.ScriptableObjects.Items;
using Assets.Scripts.Controller.Collectible;

namespace Assets.Scripts.Controller.Player
{
    public class PlayerXp : MonoBehaviour
    {
        [SerializeField]
        public FloatVariable Xp;
        [SerializeField]
        public FloatVariable MaxXP;
        [SerializeField]
        public IntVariable CurrentLevel;
        [SerializeField]
        public GameObject LevelUpEffectGO;
        [SerializeField]
        public AudioClip LevelUpAudioClip;
        [SerializeField]
        public GameEvent PlayerLevelUpEvent;

        private ParticleSystem _LevelUpEffect;
        private void Start()
        {
            _LevelUpEffect = Instantiate(LevelUpEffectGO.GetComponent<ParticleSystem>(), gameObject.transform.position, Quaternion.identity, gameObject.transform);
            CurrentLevel.value = 1;
            Xp.value = 0;
            MaxXP.value = XpToReach(CurrentLevel);
        }

        public void OnPlayerGainXp(float value) 
        {
            Xp.value += value;
            int xpToReach = XpToReach(CurrentLevel);

            while(Xp.value >= xpToReach)
            {
                AudioSource.PlayClipAtPoint(LevelUpAudioClip, transform.position, 1);
                _LevelUpEffect.Play();
                Xp.value = Xp.value - xpToReach;
                CurrentLevel.value += 1;
                MaxXP.value = XpToReach(CurrentLevel);
                PlayerLevelUpEvent.Raise();
                xpToReach = XpToReach(CurrentLevel);
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