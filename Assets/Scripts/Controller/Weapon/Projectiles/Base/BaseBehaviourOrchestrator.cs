using System.Collections;
using Assets.Scripts.Controller.Game;
using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.Types;
using System.Linq;

namespace Assets.Scripts.Controller.Weapon.Projectiles
{
    public abstract class BaseBehaviourOrchestrator : MonoBehaviour
    {
        public GameObject parent { get; set; }
        public ProjectileState currentState = ProjectileState.Start;

        public List<BaseBehaviour<Collision2D>> onCollisionBehaviours = new List<BaseBehaviour<Collision2D>>();
        public List<BaseBehaviour<float>> onEachFrameBehaviours = new List<BaseBehaviour<float>>();

        public void copyAllBehaviours(BaseBehaviourOrchestrator copy)
        {
            parent = copy.parent;
            onCollisionBehaviours.AddRange(copy.onCollisionBehaviours);
            onEachFrameBehaviours.AddRange(copy.onEachFrameBehaviours);
        }

        public void addOnCollisionBehaviour(BaseBehaviour<Collision2D> newOnCollisionBehaviour)
        {
            onCollisionBehaviours.Add(newOnCollisionBehaviour);
        }
        public void addOnEachFrameBehaviour(BaseBehaviour<float> newOnEachFrameBehaviour)
        {
            onEachFrameBehaviours.Add(newOnEachFrameBehaviour);
        }


        public T getCollisionBehaviourByType<T>() where T : BaseBehaviour<Collision2D>
        {
            foreach (BaseBehaviour<Collision2D> behaviour in onCollisionBehaviours)
            {
                if (behaviour is T)
                {
                    return (T)behaviour;
                }
            }
            return null;
        }

        public T getEachFrameBehaviourByType<T>() where T : BaseBehaviour<float>
        {
            foreach (BaseBehaviour<float> behaviour in onEachFrameBehaviours)
            {
                if (behaviour is T)
                {
                    return (T)behaviour;
                }
            }
            return null;
        }

        public void TriggerNewState(ProjectileState newState)
        {
            currentState = newState;
        }

        protected void HandleAllStartBehaviour()
        {
            foreach (BaseBehaviour<Collision2D> behaviour in onCollisionBehaviours)
            {
                behaviour.HandleStartBehaviour(this);
            }
            foreach (BaseBehaviour<float> behaviour in onEachFrameBehaviours)
            {
                behaviour.HandleStartBehaviour(this);
            }
        }

        protected void HandleAllOnCollisionBehaviour(Collision2D onCollisionPayload)
        {
            if (GameManager.GameState.State == Types.GameStateEnum.PAUSE) return;
            foreach (BaseBehaviour<Collision2D> behaviour in onCollisionBehaviours)
            {
                if (behaviour.triggeringStates != null && behaviour.triggeringStates.Contains(currentState))
                {
                    behaviour.HandleBehaviour(this, onCollisionPayload);
                }
            }
        }

        protected void HandleAllOnEachFrameBehaviour(float onEachFramePayload)
        {
            if (GameManager.GameState.State == Types.GameStateEnum.PAUSE) return;
            foreach (BaseBehaviour<float> behaviour in onEachFrameBehaviours)
            {
                if (behaviour.triggeringStates.Contains(currentState))
                {
                    behaviour.HandleBehaviour(this, onEachFramePayload);
                }
            }
        }
    }
}