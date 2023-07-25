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
        private FloatVariable xp;
        [SerializeField]
        private FloatVariable maxXp;

        private ParticleSystem _LevelUpEffect;

        void Awake()
        {
            this._LevelUpEffect = Instantiate(Resources.Load<ParticleSystem>("Prefabs/Particles/glow_1"), gameObject.transform.position, Quaternion.identity, gameObject.transform);
        }

        private void OnEnable()
        {
            xp.value = 0;
        }

        public void Collect(CollectibleSO collect)
        {
            xp.value += ((XpCollectible)collect)._xpValue;

            if (xp.value >= maxXp.value)
            {
                this._LevelUpEffect.Play();
                xp.value = xp.value - maxXp.value;
            }
        }
    }
}