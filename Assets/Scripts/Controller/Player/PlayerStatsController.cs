using UnityEngine;
using Assets.Scripts.Variables;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerStatsController : MonoBehaviour
{

    [SerializeField]
    private FloatVariable _critChance;
    [SerializeField]
    private FloatVariable _critDamage;

    private void Start()
    {

    }  

    public (bool, float) ComputeDamage(float damage)
    {
        float rand = Random.value;
        if(rand <= _critChance.value / 100f)
        {
            return (true, damage * (_critDamage.value / 100f));
        }
        return (false, damage);
    }
}
