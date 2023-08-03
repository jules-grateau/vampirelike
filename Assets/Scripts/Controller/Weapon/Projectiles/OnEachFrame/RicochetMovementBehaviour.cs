using Assets.Scripts.Controller.Weapon.Projectiles;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Assets.Scripts.Types;

public class RicochetMovementBehaviour : MovementBehaviour
{
    [SerializeField]
    public float radius;

    public override void HandleStartBehaviour(BaseBehaviourOrchestrator self)
    {
        triggeringState = ProjectileState.Start;
    }
    public override void HandleBehaviour(BaseBehaviourOrchestrator self, float time)
    {
        self.gameObject.GetComponent<Rigidbody2D>().velocity = self.transform.right * speed;
        GameObject target = GetTarget(self.transform.position, ((OnAllBehaviourOrchestrator)self).alreadyTargeted);
        if (!target) return;

        self.transform.rotation = Quaternion.Slerp(self.transform.rotation, Quaternion.LookRotation(Vector3.forward, Quaternion.Euler(0, 0, 90) * (target.transform.position - self.transform.position)), 0.8f);
    }

    private GameObject GetTarget(Vector2 shootFrom, List<GameObject> excludes)
    {
        var hits = Physics2D.OverlapCircleAll(shootFrom, radius, 1 << LayerMask.NameToLayer("Enemy"));
        if (hits.Length <= 0) return null;

        GameObject target = hits.Select(hit => new { data = hit, distance = Vector2.Distance(shootFrom, hit.transform.position) })
            .Where(hit => Physics2D.Raycast(shootFrom, hit.data.transform.position, hit.distance, 1 << LayerMask.NameToLayer("Wall")).collider == null && !excludes.Contains(hit.data.gameObject))
            .OrderBy(hit => hit.distance)
            .Select(hit => hit.data.gameObject)
            .FirstOrDefault();

        return target;
    }
}
