using Assets.Scripts.Controller.Weapon.Projectiles;
using UnityEngine;
using System.Linq;

public class AimedMovementBehaviour : MovementBehaviour
{
    [SerializeField]
    public float radius;
    GameObject _lastTarget;

    public override void HandleStartBehaviour(BaseBehaviourOrchestrator<float> self, float time)
    {
    }
    public override void HandleBehaviour(BaseBehaviourOrchestrator<float> self, float time)
    {
        self.gameObject.GetComponent<Rigidbody2D>().velocity = self.transform.right * speed;
        GameObject target = GetTarget(self.transform.position);
        if (!target) return;

        self.transform.rotation = Quaternion.Slerp(self.transform.rotation, Quaternion.LookRotation(Vector3.forward, Quaternion.Euler(0, 0, 90) * (target.transform.position - self.transform.position)), 0.8f);
        _lastTarget = target;
    }

    private GameObject GetTarget(Vector2 shootFrom)
    {
        var hits = Physics2D.OverlapCircleAll(shootFrom, radius, 1 << LayerMask.NameToLayer("Enemy"));
        if (hits.Length <= 0) return null;

        GameObject target = hits.Select(hit => new { data = hit, distance = Vector2.Distance(shootFrom, hit.transform.position) })
            .Where(hit => Physics2D.Raycast(shootFrom, hit.data.transform.position, hit.distance, 1 << LayerMask.NameToLayer("Wall")).collider == null)
            .Where(hit => hit.data.gameObject.GetInstanceID() != _lastTarget.GetInstanceID())
            .OrderBy(hit => hit.distance)
            .Select(hit => hit.data.gameObject)
            .FirstOrDefault();

        return target;
    }
}
