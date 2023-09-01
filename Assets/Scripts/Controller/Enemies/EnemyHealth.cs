using UnityEditor;
using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Types;
using System.Linq;
using Assets.Scripts.ScriptableObjects.Status;
using Assets.Scripts.Events.TypedEvents;

namespace Assets.Scripts.Controller.Enemies
{
    public class EnemyHealth : BaseHealth
    {

        [SerializeField]
        public AudioClip deathAudioClip;
        [SerializeField]
        public GameEventFloat PlayerHealEvent;

        public GameEventString EnemyDieEvent;
        public GameEventHitData LogEnemyHit;
        public string Name;

        protected override void TakeDamageEffect(HitData hit, bool isDoTTick = false)
        {
            float modifiedDamage = hit.damage * (isDoTTick ? hit.status.doTRatio : 1f);
            bool isCrit = false;

            if (hit.source != null)
            {
                PlayerStatsController stats = hit.source.GetComponent<PlayerStatsController>();
                if (stats)
                {
                    (isCrit, modifiedDamage) = stats.ComputeDamage(modifiedDamage, isDoTTick);
                }
                if (hit.status.isVampiric)
                {
                    PlayerHealEvent.Raise(modifiedDamage * hit.status.vampRatio);
                }
            }


            DisplayDamage(modifiedDamage, isCrit, hit.status);
            Health -= modifiedDamage;

            LogEnemyHit.Raise(new HitData() { damage = modifiedDamage, instanceID = hit.instanceID, 
                position = hit.position, source = hit.source, status = hit.status, weapon = hit.weapon });

            if (hit.status.canBump)
            {
                bool orientation = hit.source.transform.position.x < gameObject.transform.position.x;
                gameObject.GetComponent<Rigidbody2D>().velocity = (orientation ? 1 : -1) * hit.status.bumpForce;
            }

            if (Health <= 0)
            {
                onDeath();
            }
        }

        protected override void triggerBeforeDestroy()
        {
            AudioSource.PlayClipAtPoint(deathAudioClip, transform.position, 1);
            if (!EnemyDieEvent) return;

            EnemyDieEvent.Raise(Name);
        }
    }
}