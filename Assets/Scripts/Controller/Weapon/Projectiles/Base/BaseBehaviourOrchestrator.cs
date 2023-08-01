using System.Collections;
using Assets.Scripts.Controller.Game;
using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts.Controller.Weapon.Projectiles
{
    public abstract class BaseBehaviourOrchestrator<T> : MonoBehaviour
    {
        protected List<BaseBehaviour<T>> behaviours = new List<BaseBehaviour<T>>();

        public void addBehaviour(BaseBehaviour<T> newBehaviour)
        {
            behaviours.Add(newBehaviour);
        }

        protected void HandleAllStartBehaviour(T payload)
        {
            foreach (BaseBehaviour<T> behaviour in behaviours)
            {
                behaviour.HandleStartBehaviour(this, payload);
            }
        }

        protected void HandleAllBehaviour(T payload)
        {
            if (GameManager.GameState.State == Types.GameStateEnum.PAUSE) return;
            foreach (BaseBehaviour<T> behaviour in behaviours)
            {
                behaviour.HandleBehaviour(this, payload);
            }
        }
    }
}