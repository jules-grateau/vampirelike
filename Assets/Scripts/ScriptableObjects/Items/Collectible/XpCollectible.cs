using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects.Items.Weapons
{
    [CreateAssetMenu(fileName = "Collectible", menuName = "Collectible/Basic", order = 1)]
    public class XpCollectible : CollectibleSO
    {
        public override void Collect()
        {
            Debug.Log("COLLECTED");
        }
    }
}