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

    private void Awake()
    {
        Weapons = new List<WeaponSO>();
        Weapons.AddRange(Resources.LoadAll<WeaponSO>("ScriptableObjects/Weapons"));
        _instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject spawn in _weaponSpawnPosition)
        {
            GameObject w = weaponCollectible.GetGameObject(spawn.transform.position);
        }
    }

    public WeaponSO getRandomWeaponSO()
    {
        int random = UnityEngine.Random.Range(0, Weapons.Count);
        WeaponSO w = Weapons[random];
        Weapons.RemoveAt(random);
        return w;
    }
}
