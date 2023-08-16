using UnityEditor;
using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Types;
using System.Linq;

namespace Assets.Scripts.Controller.Enemies
{
    public class EnemyHealth : MonoBehaviour
    {
        [SerializeField]
        public float Health { get; set; }

        private GameObject _damageDisplay;

        public List<HitData> hits;

        private bool _hasDotStatus;
        private float _cooloff = 0f;

        const float dotCooldown = 1f;

        [SerializeField]
        public AudioClip deathAudioClip;
        void Awake()
        {
            _damageDisplay = Resources.Load<GameObject>("Prefabs/Particles/damage_display");
            hits = new List<HitData>();
        }

        void Update()
        {
            if (_hasDotStatus)
            {
                if (_cooloff >= dotCooldown)
                {
                    foreach (var hit in hits)
                    {
                        TakeDamageEffect(hit);
                    }
                    _cooloff = 0;
                }
                _cooloff += Time.fixedDeltaTime;
            }
        }

        private void DisplayDamage(float damage, bool isCrit, HitStatusEnum status)
        {
            GameObject dd = Instantiate(_damageDisplay);
            dd.transform.position = gameObject.transform.position + new Vector3(0, 0.5f, 0);
            dd.transform.localScale = Vector3.one;
            TextMeshPro tm = dd.GetComponentInChildren<TextMeshPro>();
            tm.SetText(damage.ToString());
            if (isCrit)
            {
                tm.gameObject.GetComponent<Animator>().Play("damage_crit");
            }
            else
            {
                tm.gameObject.GetComponent<Animator>().Play("damage_appear");
            }
            
        }

        private IEnumerator triggerNewStatus(HitData hit)
        {
            hits.Add(hit);
            _hasDotStatus = hasDoTStatus();
            yield return new WaitForSeconds(hit.time);
            hits.Remove(hit);
            _hasDotStatus = hasDoTStatus();
        }

        private void TakeDamageEffect(HitData hit)
        {
            float modifiedDamage = hit.damage;
            bool isDoT = isDoTStatus(hit.status);
            bool isCrit = false;
            PlayerStatsController stats = hit.source.GetComponent<PlayerStatsController>();
            if (stats)
            {
                (isCrit, modifiedDamage) = stats.ComputeDamage(hit.damage, isDoT);
            }

            DisplayDamage(modifiedDamage, isCrit, hit.status);
            Health -= modifiedDamage;

            switch (hit.status)
            {
                case Types.HitStatusEnum.Bump:
                    Vector2 bumpVector = (Vector2)hit.payload;
                    bool orientation = hit.source.transform.position.x < gameObject.transform.position.x;
                    gameObject.GetComponent<Rigidbody2D>().velocity = (orientation ? 1 : -1) * bumpVector;
                    break;
                default:
                    break;
            }

            if (Health <= 0)
            {

                DestructibleController destructible = gameObject.GetComponent<DestructibleController>();
                if (destructible)
                {
                    destructible.onDestroy();
                }
                DropCollectible drop = gameObject.GetComponent<DropCollectible>();
                if (drop)
                {
                    drop.onDestroy();
                }
                AudioSource.PlayClipAtPoint(deathAudioClip, transform.position, 1);
                Destroy(gameObject);
            }
        }

        public void TakeDamage(HitData hit)
        {
            if (gameObject.GetInstanceID() == hit.instanceID && hit.source)
            {
                StartCoroutine(triggerNewStatus(hit));
                TakeDamageEffect(hit);
            }
        }

        private bool hasDoTStatus()
        {
            return hits.Select(h => h.status).Where(s => isDoTStatus(s)).ToList().Count > 0;
        }

        private bool isDoTStatus(HitStatusEnum s)
        {
            return s.Equals(HitStatusEnum.Fire) ||
                    s.Equals(HitStatusEnum.Poison);
        }

        public bool hasImpairment()
        {
            return hits.Select(h => h.status).Where(s => isImpairment(s)).ToList().Count > 0;
        }

        private bool isImpairment(HitStatusEnum s)
        {
            return s.Equals(HitStatusEnum.Bump) ||
                    s.Equals(HitStatusEnum.Stun);
        }
    }
}