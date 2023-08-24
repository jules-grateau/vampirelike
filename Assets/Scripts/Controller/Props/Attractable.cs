using UnityEditor;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

namespace Assets.Scripts.Controller.Props
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Attractable : MonoBehaviour
    {
        public bool IsAttractable { get; set; }
        private Transform _attractorTranform;
        private Rigidbody2D _rigidbody;
        [SerializeField]
        private float _speed = 6f;

        public void Attract(Transform attactorTransform)
        {
            _attractorTranform = attactorTransform;
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (!IsAttractable) return;
            if (!_attractorTranform) return;
            if (!_rigidbody) return;


            float distance = Vector2.Distance(_attractorTranform.transform.position, transform.position);

            var direction = _attractorTranform.position - transform.position;

            float flow = Mathf.Max(distance * 0.5f, 1f);
            _rigidbody.velocity = direction.normalized * _speed * flow;
        }
    }
}