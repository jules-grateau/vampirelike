using Assets.Scripts.Controller.Enemies;
using Assets.Scripts.Controller.Enemies.Interface;
using Assets.Scripts.Events.TypedEvents;
using Assets.Scripts.ScriptableObjects.Items;
using Assets.Scripts.Types;
using System.Collections;
using UnityEngine.Events;
using UnityEngine;
using Assets.Scripts.ScriptableObjects.Status;
using Assets.Scripts.Controller.Collectible;

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
        GameEventHitData _collisionEvent;

        [SerializeField]
        float _damage;
        [SerializeField]
        StatusSO _statusDamages;

        [Header("Health")]
        [SerializeField]
        public float health;
        [SerializeField]
        GameEventHitData _enemyHitEvent;
        [SerializeField]
        GameEventFloat _playerHealEvent;

        [Header("Xp")]
        [SerializeField]
        private bool _dropXp;
        [SerializeField]
        [DrawIf("_dropXp", true, ComparisonType.Equals, DisablingType.DontDraw)]
        XpCollectibleSO _xpCollectible;
        [SerializeField]
        [DrawIf("_dropXp", true, ComparisonType.Equals, DisablingType.DontDraw)]
        float _xpValue;

        [Header("Drop")]
        [SerializeField]
        EnemyDrop _dropType;
        [SerializeField]
        [DrawIf("_dropType", EnemyDrop.Collecticle, ComparisonType.Equals, DisablingType.DontDraw)]
        BaseCollectibleSO[] _collectibles;


        [SerializeField]
        AudioClip _deathAudioClip;

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
                case EnemyBehavior.GatherCollectible:
                    movementScript = enemy.AddComponent<SeekCollectibleController>();
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
                damageScript.Status = _statusDamages;
            }

            DropCollectible dropCollectible = enemy.AddComponent<DropCollectible>();
            if (_dropXp && _xpValue > 0)
            {
                dropCollectible.addCollectibleFunction((Vector3 pos) => _xpCollectible.GetGameObject(pos, _xpValue));
            }

            switch (_dropType)
            {
                case EnemyDrop.Collecticle:
                    foreach (BaseCollectibleSO _collectible in _collectibles)
                    {
                        if (Random.value <= _collectible.dropChance)
                        {
                            dropCollectible.addCollectibleFunction((Vector3 pos) => _collectible.GetGameObject(pos));
                        }
                    }
                    break;
                case EnemyDrop.None:
                default:
                    break;
            }

            DestructibleController destructibleController = enemy.AddComponent<DestructibleController>();
            EnemyHealth enemyHealth = enemy.AddComponent<EnemyHealth>();
            enemyHealth.Health = health;
            enemyHealth.deathAudioClip = _deathAudioClip;
            enemyHealth.PlayerHealEvent = _playerHealEvent;

            GameEventListenerHitData listenerHitData = enemy.AddComponent<GameEventListenerHitData>();
            listenerHitData.GameEvent = _enemyHitEvent;
            listenerHitData.UnityEvent = new UnityEvent<HitData>();
            listenerHitData.UnityEvent.AddListener(enemyHealth.TakeDamage);

            Animator animator = enemy.GetComponentInChildren<Animator>();
            animator.speed = _speed / 2;

            return enemy;

        }

        public Vector2 GetSize()
        {
            BoxCollider2D collider = _prefab.GetComponentInChildren<BoxCollider2D>();
            if (!collider) return new Vector2(0, 0);

            return collider.size;
        }

        public Vector2 GetColliderOffset()
        {
            BoxCollider2D collider = _prefab.GetComponentInChildren<BoxCollider2D>();
            if (!collider) return new Vector2(0, 0);

            return collider.offset;
        }

    }
}