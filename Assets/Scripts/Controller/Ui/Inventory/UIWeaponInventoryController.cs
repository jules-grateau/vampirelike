using Assets.Scripts.ScriptableObjects.Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIWeaponInventoryController : MonoBehaviour
{
    private GameObject _inventorySlotPrefab;

    public void Start()
    {
        _inventorySlotPrefab = (GameObject) Resources.Load("Prefabs/UI/WeaponSlot");
    }

    public void AddWeapon(WeaponSO weapon)
    {
        GameObject inventorySlot = Instantiate(_inventorySlotPrefab, gameObject.transform);
        inventorySlot.GetComponent<UIWeaponCooldownController>().SetWeaponSO(weapon);
        Image image = inventorySlot.transform.Find("Icon").GetComponent<Image>();
        image.overrideSprite = weapon.icon;
    }
}
