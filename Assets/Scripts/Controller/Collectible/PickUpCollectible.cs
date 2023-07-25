using Assets.Scripts.Controller.Weapon;
using Assets.Scripts.Events.TypedEvents;
using Assets.Scripts.ScriptableObjects.Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PickUpCollectible : MonoBehaviour
{
    [SerializeField]
    private GameEventCollectible gameEvent;
    [SerializeField]
    private CollectibleSO collectible;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            gameEvent.Raise(collectible);
            collectible.Collect(transform.position);
            Destroy(gameObject);
        }
    }
}
