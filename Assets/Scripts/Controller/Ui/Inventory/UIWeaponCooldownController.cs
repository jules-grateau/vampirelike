using Assets.Scripts.ScriptableObjects.Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIWeaponCooldownController : MonoBehaviour
{
    private Image _loadingImage;
    private WeaponSO _weapon;

    public void Start()
    {
        _loadingImage = gameObject.GetComponent<Image>();
    }

    public void SetWeaponSO(WeaponSO weapon)
    {
        _weapon = weapon;
    }

    private void Update()
    {
        if (!_loadingImage || !_weapon) return;
        _loadingImage.fillAmount = Mathf.Clamp01(Mathf.InverseLerp(0, _weapon.GetCooldown(), _weapon.cooloff));
    }
}
