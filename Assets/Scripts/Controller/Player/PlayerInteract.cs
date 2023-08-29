using Assets.Scripts.Events;
using Assets.Scripts.Variables;
using Assets.Scripts.Variables.Constants;
using System.Collections;
using Assets.Scripts.Controller.Game;
using UnityEngine;
using Assets.Scripts.ScriptableObjects.Items;
using System.Linq;

namespace Assets.Scripts.Controller.Player
{
    public class PlayerInteract : MonoBehaviour
    {
        [SerializeField]
        private float _radius = 2.5f;

        private GameObject currentTarget;

        private void Update()
        {
            var hits = Physics2D.OverlapCircleAll(gameObject.transform.position, _radius, 1 << LayerMask.NameToLayer("Interactible"));
            if (hits.Length <= 0)
            {
                if (currentTarget)
                {
                    SpriteRenderer spriteOldTarget = currentTarget.GetComponent<SpriteRenderer>();
                    spriteOldTarget.material.SetInt("_Interact", 0);
                    currentTarget = null;
                }
                return;
            }

            GameObject target = hits.Select(hit => new { data = hit, distance = Vector2.Distance(gameObject.transform.position, hit.transform.position) })
                .Where(hit =>
                {
                    RaycastHit2D hasDirectPath = Physics2D.Raycast(gameObject.transform.position, (hit.data.transform.position - gameObject.transform.position).normalized, hit.distance, 1 << LayerMask.NameToLayer("Wall"));
                    if (hasDirectPath.collider)
                    {
                        Debug.DrawLine(gameObject.transform.position, hasDirectPath.point, Color.green, 1f);
                        Debug.DrawLine(hasDirectPath.point, hit.data.transform.position, Color.red, 1f);
                        return false;
                    };
                    Debug.DrawLine(gameObject.transform.position, hit.data.transform.position, Color.green, 1f);
                    return true;
                })
                .OrderBy(hit => hit.distance)
                .Select(hit => hit.data.gameObject)
                .FirstOrDefault();

            if (target && !target.Equals(currentTarget))
            {
                if (currentTarget)
                {
                    SpriteRenderer spriteOldTarget = currentTarget.GetComponent<SpriteRenderer>();
                    spriteOldTarget.material.SetInt("_Interact", 0);
                    currentTarget = null;
                }

                SpriteRenderer sprite = target.GetComponent<SpriteRenderer>();
                sprite.material.SetInt("_Interact", 1);
                currentTarget = target;
            }
        }
    }
}