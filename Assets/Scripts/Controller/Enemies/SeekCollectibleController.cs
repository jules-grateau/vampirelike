using Assets;
using Assets.Scripts.Controller.Enemies;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Types;
using System.Linq;
using Assets.Scripts.Controller.Collectible;

[RequireComponent(typeof(Rigidbody2D))]
public class SeekCollectibleController : IEnemyMovement
{
    [SerializeField]
    private GameObject _player;
    [SerializeField]
    private float pickupCooldown = 0.5f;
    private Rigidbody2D _rigidbody;
    private EnemyHealth _enemyHealth;
    private GameObject _focus;
    protected float _cooloff = 0f;
    protected const float seekCooldown = 5f;

    private bool _isFlipped = false;
    private bool _isPickingLoot = false;

    public Queue<GameObject> Loots;

    // Start is called before the first frame update
    void Start()
    {
        Loots = new Queue<GameObject>();
        _player = GameObject.FindGameObjectWithTag("Player");
        _rigidbody = GetComponent<Rigidbody2D>();
        _enemyHealth = GetComponent<EnemyHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_enemyHealth.hasStun()) return;
        if (_isPickingLoot)
        {
            _rigidbody.velocity = Vector2.zero;
            return;
        }
        
        float modifiedSpeed = Speed * _enemyHealth.getSlowValue();

        // Seek a new focus when cooldown is ended or no more _focus
        if (_cooloff >= seekCooldown || !_focus)
        {
            _focus = GetClosestCollectible(gameObject.transform.position, _player);
            _cooloff = 0;
        }

        //TODO : Add behavior when nothing to collect
        if (!_focus) return;
        Debug.DrawLine(transform.position, _focus.transform.position);

        Vector3 direction = _focus.transform.position - transform.position;
        if (_focus.Equals(_player))
        {
            direction = -direction;
        }
        _rigidbody.velocity = direction.normalized * modifiedSpeed;
        bool needToFlip = direction.normalized.x < 0;

        if (needToFlip != _isFlipped)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            _isFlipped = needToFlip;
        }
        _cooloff += Time.fixedDeltaTime;
    }

    public void PickUp(GameObject item)
    {
        item.SetActive(false);
        Loots.Enqueue(item);
        _focus = null;
        StartCoroutine(triggerPickup());
    }

    private IEnumerator triggerPickup()
    {
        _isPickingLoot = true;
        yield return new WaitForSeconds(pickupCooldown);
        _isPickingLoot = false;
    }

    private GameObject GetClosestCollectible(Vector2 shootFrom, GameObject player)
    {
        var hits = Physics2D.OverlapCircleAll(shootFrom, float.PositiveInfinity, 1 << LayerMask.NameToLayer("Collectible"));
        if (hits.Length <= 0) return player;

        GameObject target = hits.Select(hit => new { data = hit, distance = Vector2.Distance(shootFrom, hit.transform.position) })
            .Where(hit =>
            {
                RaycastHit2D hasDirectPath = Physics2D.Raycast(shootFrom, (hit.data.transform.position - (Vector3)shootFrom).normalized, hit.distance, 1 << LayerMask.NameToLayer("Wall"));
                if (hasDirectPath.collider)
                {
                    Debug.DrawLine(shootFrom, hasDirectPath.point, Color.green, 1f);
                    Debug.DrawLine(hasDirectPath.point, hit.data.transform.position, Color.red, 1f);
                    return false;
                };
                Debug.DrawLine(shootFrom, hit.data.transform.position, Color.green, 1f);
                return true;
            })
            .OrderBy(hit => hit.distance)
            .Select(hit => hit.data.gameObject)
            .FirstOrDefault();

        if (player && target)
        {
            float distToPlayer = Vector2.Distance(shootFrom, player.transform.position);
            float distToTarget = Vector2.Distance(shootFrom, target.transform.position);
            return distToPlayer < distToTarget ? player : target;
        }
        if (target) return target;
        if (player) return player;
        return null;
    }
}
