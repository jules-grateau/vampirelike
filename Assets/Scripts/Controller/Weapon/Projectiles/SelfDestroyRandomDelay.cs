using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Controller.Weapon.Projectiles
{
    public class SelfDestroyRandomDelay : MonoBehaviour
    {

        [SerializeField]
        public float minDelay;
        [SerializeField]
        public float maxDelay;

        // Use this for initialization
        void Start()
        {
            Destroy(gameObject, Random.Range(minDelay, maxDelay));
        }
    }
}