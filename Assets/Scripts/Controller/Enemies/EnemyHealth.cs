using UnityEditor;
using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Types;
using System.Linq;
using Assets.Scripts.ScriptableObjects.Status;

namespace Assets.Scripts.Controller.Enemies
{
    public class EnemyHealth : MonoBehaviour
    {
        [SerializeField]
        public float Health { get; set; }

        public Transform statusAnchor { get; set; }

        private GameObject _damageDisplay;

        private GameObject _anchor;

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
            buildStatusAnchor();
        }

        void Update()
        {
            if (_hasDotStatus)
            {
                if (_cooloff >= dotCooldown)
                {
                    foreach (var hit in hits)
                    {
                        TakeDamageEffect(hit, true);
                    }
                    _cooloff = 0;
                }
                _cooloff += Time.fixedDeltaTime;
            }
        }

        private void buildStatusAnchor()
        {
            _anchor = new GameObject("status_anchor");
            _anchor.AddComponent<SpriteRenderer>();
            _anchor.transform.position = gameObject.transform.position + new Vector3(0, gameObject.GetComponentInChildren<SpriteRenderer>().sprite.bounds.size.y / 2 + 0.5f, 0);
            _anchor.transform.parent = gameObject.transform;
            _anchor.transform.localScale = new Vector3(0.5f, 0.5f, 1);
        }

        private void DisplayDamage(float damage, bool isCrit, StatusSO status)
        {
            GameObject dd = Instantiate(_damageDisplay);
            dd.transform.position = gameObject.transform.position + new Vector3(0, 0.5f, 0);
            dd.transform.localScale = Vector3.one;
            TextMeshPro tm = dd.GetComponentInChildren<TextMeshPro>();
            string hexColor = ColorUtility.ToHtmlStringRGB(status.color);
            if (isCrit)
            {
                tm.gameObject.GetComponent<Animator>().Play("damage_crit");
            }
            else
            {
                tm.gameObject.GetComponent<Animator>().Play("damage_appear");
            }
            tm.SetText("<#" + hexColor + ">" + damage.ToString());
        }

        private IEnumerator triggerNewStatus(HitData hit)
        {
            hits.Add(hit);
            _hasDotStatus = hasDoTStatus();
            SpriteRenderer sp = _anchor.GetComponent<SpriteRenderer>();
            sp.sprite = hit.status?.sprite ?? null;
            if (hit.status.isDoT)
            {
                yield return new WaitForSeconds(hit.status.doTTime);
            }
            else if (hit.status.isStun)
            {
                yield return new WaitForSeconds(hit.status.stunTime);
            }
            else if (hit.status.isSlow)
            {
                yield return new WaitForSeconds(hit.status.slowTime);
            }
            hits.Remove(hit);
            _hasDotStatus = hasDoTStatus();
            sp.sprite = null;
        }

        private void TakeDamageEffect(HitData hit, bool isDoTTick = false)
        {
            float modifiedDamage = hit.damage * (isDoTTick ? hit.status.doTRatio : 1f);
            bool isCrit = false;
            PlayerStatsController stats = hit.source.GetComponent<PlayerStatsController>();
            if (stats)
            {
                (isCrit, modifiedDamage) = stats.ComputeDamage(modifiedDamage, isDoTTick);
            }

            DisplayDamage(modifiedDamage, isCrit, hit.status);
            Health -= modifiedDamage;

            if(hit.status.canBump)
            {
                bool orientation = hit.source.transform.position.x < gameObject.transform.position.x;
                gameObject.GetComponent<Rigidbody2D>().velocity = (orientation ? 1 : -1) * hit.status.bumpForce;
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
            return hits.Select(h => h.status).Where(s => s.isDoT).ToList().Count > 0;
        }


        public bool hasStun()
        {
            return hits.Select(h => h.status).Where(s => s.isStun).ToList().Count > 0;
        }

        public float getSlowValue()
        {
            StatusSO slowStatus = hits.Select(h => h.status).Where(s => s.isSlow).OrderBy(s => s.slowValue).FirstOrDefault();
            return !slowStatus ? 1f : slowStatus.slowValue;
        }
    }
}