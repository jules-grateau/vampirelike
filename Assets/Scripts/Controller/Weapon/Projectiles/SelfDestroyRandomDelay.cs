using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Controller.Weapon.Projectiles
{
    public class SelfDestroyRandomDelay : MonoBehaviour
    {

        [SerializeField]
        private float _minDelay;
        [SerializeField]
        private float _maxDelay;

        // Use this for initialization
        void Start()
        {
            Destroy(gameObject, Random.Range(_minDelay, _maxDelay));
        }
    }
}