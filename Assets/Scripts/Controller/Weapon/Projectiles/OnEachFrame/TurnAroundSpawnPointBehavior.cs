using Assets.Scripts.Controller.Weapon.Projectiles;
using UnityEngine;
using Assets.Scripts.Types;

public class TurnAroundSpawnPointBehavior : MovementBehaviour
{
    Vector2 _spawnPosition;
    public float Radius { get; set; }
    public float Duration { get; set; }
    public float BaseDuration { get; set; }
    public float BaseSpeed { get; set; }
    public float SpeedPercentage { get; set; }

    float _aliveTime;
    float _localAngle;
    float _minRadius = 2f;
    public override void HandleStartBehaviour(BaseBehaviourOrchestrator self)
    {
        triggeringStates = new ProjectileState[] { ProjectileState.Start };
        _spawnPosition = self.transform.position;
    }
    public override void HandleBehaviour(BaseBehaviourOrchestrator self, float time)
    {
        _aliveTime += time;

        float computedRadius = (Radius * (_aliveTime) + _minRadius) / (BaseDuration * (1 + (Duration / 100)));

        _localAngle += (time * BaseSpeed * (1 + (SpeedPercentage / 100))) * (Radius / computedRadius);

        self.transform.position = new Vector2(_spawnPosition.x + Mathf.Cos(_localAngle) * computedRadius, 
            _spawnPosition.y + Mathf.Sin(_localAngle) * computedRadius );

        if (_localAngle >= 360) _localAngle = 0;
    }
}
