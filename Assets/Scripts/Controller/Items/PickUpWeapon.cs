using Assets.Scripts.Controller.Weapon;
using Assets.Scripts.Events.TypedEvents;
using Assets.Scripts.ScriptableObjects.Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PickUpWeapon : MonoBehaviour
{
    [SerializeField]
    private GameEventWeapon gameEvent;
    [SerializeField]
    private WeaponSO weapon;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            gameEvent.Raise(weapon);
            Destroy(gameObject);
        }

    }
}
