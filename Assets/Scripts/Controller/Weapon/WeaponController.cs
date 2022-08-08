using Assets.Scripts.Controller.Weapon;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public Weapon weapon;
    private float cooloff = 0f;

    // Update is called once per frame
    void Update()
    {
        if (weapon == null) return;

        if(cooloff >= weapon.cooldown)
        {
            weapon.Use(gameObject.transform.position);
            cooloff = 0;
        }
        cooloff += Time.deltaTime;
    }
}
