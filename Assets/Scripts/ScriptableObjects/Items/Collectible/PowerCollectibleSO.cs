using UnityEditor;
using UnityEngine;
using System.Collections;

namespace Assets.Scripts.ScriptableObjects.Items
{
    [CreateAssetMenu(fileName = "Collectible", menuName = "Collectible/Power", order = 1)]
    public abstract class PowerCollectibleSO : CollectibleSO
    {
        [SerializeField]
        public float _duration;
    }
}