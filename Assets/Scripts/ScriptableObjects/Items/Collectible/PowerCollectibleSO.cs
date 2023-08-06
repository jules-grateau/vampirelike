using UnityEditor;
using UnityEngine;
using Assets.Scripts.Controller.Collectible;
using System.Collections;

namespace Assets.Scripts.ScriptableObjects.Items
{
    [CreateAssetMenu(fileName = "Collectible", menuName = "Collectible/Power", order = 1)]
    public abstract class PowerCollectibleSO<T> : CollectibleSO<T> where T : PowerCollectible
    {
        [SerializeField]
        public float _duration;

        public override T GetCollectible(Vector3 position)
        {
            T powerCollectibleController = base.GetCollectible(position);
            powerCollectibleController.Duration = _duration;

            return powerCollectibleController;
        }
    }
}