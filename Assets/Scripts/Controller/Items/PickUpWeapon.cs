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
    private GameEventWeapon _gameEvent;
    [SerializeField]
    private WeaponSO _weapon;
    [SerializeField]
    private AudioClip _defaultAudioClip;

    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = _weapon.icon;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            AudioSource.PlayClipAtPoint(_defaultAudioClip, transform.position, 1);
            _weapon.parent = collision.gameObject;
            _weapon.Init();
            _gameEvent.Raise(_weapon);
            Destroy(gameObject);
        }
    }
}
