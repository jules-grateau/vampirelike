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

        GameObject focus = GetClosestCollectible(gameObject.transform.position, _player);
        //TODO : Add behavior when nothing to collect
        if (!focus) return;
        Debug.DrawLine(transform.position, focus.transform.position);

        Vector3 direction = focus.transform.position - transform.position;
        if (focus.Equals(_player))
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
    }

    public void PickUp(GameObject item)
    {
        item.SetActive(false);
        Loots.Enqueue(item);
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
        if (hits.Length <= 0) return null;

        GameObject target = hits.Select(hit => new { data = hit, distance = Vector2.Distance(shootFrom, hit.transform.position) })
            .Where(hit => Physics2D.Raycast(shootFrom, hit.data.transform.position, hit.distance, 1 << LayerMask.NameToLayer("Wall")).collider == null)
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
