using UnityEditor;
using UnityEngine;
using TMPro;

namespace Assets.Scripts.Controller.Enemies
{
    public class EnemyHealth : MonoBehaviour
    {
        [SerializeField]
        public float Health { get; set; }

        private GameObject _damageDisplay;

        void Awake()
        {
            _damageDisplay = Resources.Load<GameObject>("Prefabs/Particles/damage_display");
        }
        
        private void DisplayDamage(float damage, bool isCrit)
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

        public void TakeDamage(HitData hit)
        {
            if(gameObject.GetInstanceID() == hit.instanceID && hit.source)
            {
               float modifiedDamage = hit.damage;
                bool isCrit = false;
                PlayerStatsController stats = hit.source.GetComponent<PlayerStatsController>();
                if (stats)
                {
                    (isCrit, modifiedDamage) = stats.ComputeDamage(hit.damage);
                }

                DisplayDamage(modifiedDamage, isCrit);
                Health -= modifiedDamage;
            }

            if(Health <= 0)
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
                Destroy(gameObject);
            }
        }
    }
}