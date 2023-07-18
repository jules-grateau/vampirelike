using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Controller.Enemies
{
    public class EnemyHealth : MonoBehaviour
    {
        [SerializeField]
        private float _health;

        public void TakeDamage(HitData hit)
        {
            Debug.Log($"Enemy with instanceId {gameObject.GetInstanceID()} recieved hit for {hit.instanceID} - {hit.damage} damages");
            if(gameObject.GetInstanceID() == hit.instanceID)
            {
                _health -= hit.damage;
            }

            if(_health <= 0)
            {
                DestructibleController destructible = gameObject.GetComponent<DestructibleController>();
                if (destructible)
                {
                    destructible.onDestroy();
                }
                Destroy(gameObject);
            }
        }
    }
}