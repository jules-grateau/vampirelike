using UnityEngine;
using Assets.Scripts.Variables;
using Assets.Scripts.ScriptableObjects;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerStatsController : MonoBehaviour
{

    [SerializeField]
    private FloatVariable _critChance;
    [SerializeField]
    private FloatVariable _critDamage;
    [SerializeField]
    private FloatVariable _baseDamage;
    [SerializeField]
    private FloatVariable _damagePercentage;
    [SerializeField]
    private FloatVariable _maxHp;
    [SerializeField]
    private FloatVariable _pickUpRadius;


    [SerializeField]
    private float _baseCritChance;
    [SerializeField]
    private float _baseCritDamage;
    [SerializeField]
    private float _baseBaseDamage;
    [SerializeField]
    private float _baseDamagePercentage;
    [SerializeField]
    private float _baseMaxHp;
    [SerializeField]
    private float _basePickUpRadius;


    private void Awake()
    {
        _critChance.value = _baseCritChance;
        _critDamage.value = _baseCritDamage;
        _baseDamage.value = _baseBaseDamage;
        _damagePercentage.value = _baseDamagePercentage;
        _maxHp.value = _baseMaxHp;
        _pickUpRadius.value = _basePickUpRadius;
    }

    public (bool, float) ComputeDamage(float damage)
    {
        float rand = Random.value;
        bool isCrit = rand <= _critChance.value / 100f;
        float computedDamage = damage;

        //Base Damage
        computedDamage += _baseDamage.value;
        //Damage Percentage
        computedDamage *= (1 + (_damagePercentage.value / 100));

        if (isCrit)
        {
            computedDamage = computedDamage * (_critDamage.value / 100f);
        }


        return (isCrit, computedDamage);
    }

    public void OnSelectUpgrade(UpgradeSO upgrade)
    {
        upgrade.Consume();
    }
}
