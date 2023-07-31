using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.ScriptableObjects.Items;
using UnityEngine;

public class WeaponSpawnerController : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _weaponSpawnPosition;
    // Start is called before the first frame update
    void Start()
    {
        GameObject weaponPrefab = Resources.Load<GameObject>("Prefabs/Weapon/Weapon");
        List<WeaponSO> weapons = new List<WeaponSO>();
        weapons.AddRange(Resources.LoadAll<WeaponSO>("ScriptableObjects/Weapons"));

        foreach (GameObject spawn in _weaponSpawnPosition)
        {
            int random = UnityEngine.Random.Range(0, weapons.Count);
            GameObject w = Instantiate(weaponPrefab, spawn.transform.position, Quaternion.identity);
            w.GetComponent<PickUpWeapon>().weapon = weapons[random];
            weapons.RemoveAt(random);
        }
    }
}
