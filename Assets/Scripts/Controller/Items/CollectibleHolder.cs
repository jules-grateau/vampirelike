using Assets.Scripts.ScriptableObjects.Items;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Controller.Items
{
    public class CollectibleHolder : MonoBehaviour
    {
        [SerializeField]
        private CollectibleSO collectible;

        private void Start()
        {
            GameObject instance = collectible.GetCollectible();
            instance.transform.position = transform.position;
            instance.SetActive(true);

        }
    }
}