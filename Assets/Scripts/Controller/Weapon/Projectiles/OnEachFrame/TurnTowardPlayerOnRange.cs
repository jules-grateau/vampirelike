﻿using Assets.Scripts.Controller.Game;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Controller.Weapon.Projectiles.OnEachFrame
{
    public class TurnTowardPlayerOnRange : StraightMovementBehaviour
    {
        public float Range;

        public override void HandleBehaviour(BaseBehaviourOrchestrator<float> self, float payload)
        {
            base.HandleBehaviour(self, payload);
            float distance = Vector2.Distance(self.transform.position, GameManager.GameState.Player.transform.position); 
            if (distance < Range) return;
            Vector2 direction = GameManager.GameState.Player.transform.position - self.transform.position;
            self.transform.right = direction.normalized;

        }

        public override void HandleStartBehaviour(BaseBehaviourOrchestrator<float> self, float payload)
        {
        }


    }
}