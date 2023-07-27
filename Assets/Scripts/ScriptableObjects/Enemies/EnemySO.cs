﻿using Assets.Scripts.Controller.Enemies;
using Assets.Scripts.Controller.Enemies.Interface;
using Assets.Scripts.Events.TypedEvents;
using Assets.Scripts.ScriptableObjects.Items;
using Assets.Scripts.Types;
using System.Collections;
using UnityEngine.Events;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects.Enemies
{
    [CreateAssetMenu(fileName = "Enemy", menuName = "Enemy/Basic", order = 9)]
    public class EnemySO : ScriptableObject
    {
        [Header("Behavior")]
        [SerializeField]
        EnemyBehavior _behaviorType;
        [SerializeField]
        float _speed;

        [Header("Damage")]
        [SerializeField]
        EnemyDamage _damageType;
        [SerializeField]
        [DrawIf("_damageType", EnemyDamage.Collision, ComparisonType.Equals, DisablingType.DontDraw)]
        GameEventFloat _collisionEvent;

        [SerializeField]
        float _damage;

        [Header("Health")]
        [SerializeField]
        float _health;
        [SerializeField]
        GameEventHitData _enemyHitEvent;

        [Header("Drop")]
        [SerializeField]
        EnemyDrop _dropType;
        [SerializeField]
        [DrawIf("_dropType", EnemyDrop.Collecticle, ComparisonType.Equals, DisablingType.DontDraw)]
        GameObject _collectible;

        [SerializeField]
        GameObject _prefab;

        public GameObject GetEnemy()
        {
            GameObject enemy = Instantiate(_prefab);
            enemy.SetActive(false);

            IEnemyMovement movementScript;
            switch(_behaviorType)
            {
                case EnemyBehavior.FollowPlayer:
                    movementScript = enemy.AddComponent<FollowPlayerController>();
                    break;
                default:
                    movementScript = null;
                    break;
            }

            if(movementScript)
            {
                movementScript.Speed = _speed;
            }

            IEnemyDamage damageScript;
            switch(_damageType)
            {
                case EnemyDamage.Collision:
                    DamagePlayerOnCollision collisionScript =  enemy.AddComponent<DamagePlayerOnCollision>();
                    collisionScript.PlayerColisionEvent = _collisionEvent;
                    damageScript = collisionScript;
                    break;
                default:
                    damageScript = null;
                    break;
            }
            if(damageScript)
            {
                damageScript.Damage = _damage;
            }

            DropCollectible dropCollectible;
            switch (_dropType)
            {
                case EnemyDrop.Collecticle:
                    dropCollectible = enemy.AddComponent<DropCollectible>();
                    dropCollectible.Collectible = _collectible;
                    break;
                case EnemyDrop.Xp:
                    dropCollectible = enemy.AddComponent<DropCollectible>();
                    dropCollectible.Collectible = Resources.Load<GameObject>("Prefabs/Collectibles/soul_1");
                    break;
                case EnemyDrop.None:
                default:
                    break;
            }

            EnemyHealth enemyHealth = enemy.AddComponent<EnemyHealth>();
            enemyHealth.Health = _health;
            GameEventListenerHitData listenerHitData = enemy.AddComponent<GameEventListenerHitData>();
            listenerHitData.GameEvent = _enemyHitEvent;
            listenerHitData.UnityEvent = new UnityEvent<HitData>();
            listenerHitData.UnityEvent.AddListener(enemyHealth.TakeDamage);

            return enemy;

        }

    }
}