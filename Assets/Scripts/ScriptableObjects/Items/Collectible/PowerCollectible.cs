using UnityEditor;
using UnityEngine;
using System.Collections;

namespace Assets.Scripts.ScriptableObjects.Items
{
    [CreateAssetMenu(fileName = "Collectible", menuName = "Collectible/Power", order = 1)]
    public abstract class PowerCollectible : CollectibleSO
    {
        [SerializeField]
        public float _duration;

        public override void Collect(Vector3 position)
        {
            Power.instance.StartCoroutine(TriggerCollect(position));
        }

        public abstract void CollectON(Vector3 position);

        public abstract void CollectOFF(Vector3 position);

        private IEnumerator TriggerCollect(Vector3 position)
        {
            CollectON(position);
            yield return new WaitForSeconds(_duration);
            CollectOFF(position);
        }
    }
}