using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.ScriptableObjects.Items;
using UnityEngine;
using Assets.Scripts.Controller.Collectible;

public class WeaponSpawnedController : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _weaponSpawnPosition;
    [SerializeField]
    private WeaponCollectibleSO weaponCollectible;

    private static WeaponSpawnedController _instance;
    public static WeaponSpawnedController Instance => _instance;

    public List<WeaponSO> Weapons;

    public void Init()
    {
        Weapons = new List<WeaponSO>();
        Weapons.AddRange(Resources.LoadAll<WeaponSO>("ScriptableObjects/Weapons"));
        _instance = this;
    }

    public void SpawnWeapons()
    {
        foreach (GameObject spawn in _weaponSpawnPosition)
        {
            GameObject w = weaponCollectible.GetGameObject(spawn.transform.position);
        }
    }

    public WeaponSO getRandomWeaponSO()
    {
        int random = UnityEngine.Random.Range(0, Weapons.Count - 1);
        WeaponSO w = Weapons[random];
        Weapons.RemoveAt(random);
        return w;
    }

    public void RemoveWeaponFromPool(WeaponSO weapon)
    {
        Weapons.Remove(weapon);
    }
}
