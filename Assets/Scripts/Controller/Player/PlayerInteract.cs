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
        private float _radius;

        private void Update()
        {
            var hits = Physics2D.OverlapCircleAll(gameObject.transform.position, _radius, 1 << LayerMask.NameToLayer("Interactible"));
            if (hits.Length <= 0) return;

            GameObject target = hits.Select(hit => new { data = hit, distance = Vector2.Distance(gameObject.transform.position, hit.transform.position) })
                .Where(hit => Physics2D.Raycast(gameObject.transform.position, hit.data.transform.position, hit.distance, 1 << LayerMask.NameToLayer("Wall")).collider == null)
                .OrderBy(hit => hit.distance)
                .Select(hit => hit.data.gameObject)
                .FirstOrDefault();


        }
    }
}