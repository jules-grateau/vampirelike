using UnityEditor;
using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Types;
using System.Linq;
using Assets.Scripts.ScriptableObjects.Status;

namespace Assets.Scripts.Controller
{
    public abstract class BaseHealth : MonoBehaviour
    {
        [SerializeField]
        public float Health;
        public List<HitData> hits;
        public Transform statusAnchor;

        protected GameObject _damageDisplay;
        protected GameObject _anchor;
        protected bool _hasDotStatus;
        protected float _cooloff = 0f;

        protected const float dotCooldown = 1f;

        public virtual void Awake()
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

        protected void DisplayDamage(float damage, bool isCrit, StatusSO status)
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

        protected abstract void triggerBeforeDestroy();

        protected abstract void TakeDamageEffect(HitData hit, bool isDoTTick = false);

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

        protected virtual void onDeath()
        {
            DestructibleController destructible = gameObject.GetComponent<DestructibleController>();
            if (destructible)
            {
                destructible.OnDestruction();
            }
            DropCollectible drop = gameObject.GetComponent<DropCollectible>();
            if (drop)
            {
                drop.OnDropCollectible();
            }
            triggerBeforeDestroy();
            Destroy(gameObject);
        }
    }
}