using System;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects.Items.Weapons
{
    [CreateAssetMenu(fileName = "Trail weapon", menuName = "Weapon/Trail", order = 2)]
    public class TrailWeapon : ProjectileWeapon
    {
       [SerializeField]
        private Vector2 _minOffset;
        [SerializeField]
        private Vector2 _maxOffset;
        [SerializeField]
        private int _numberPerLine;
        [SerializeField]
        private float _minSpacing;
        [SerializeField]
        private float _maxSpacing;

        private GameObject trailContainer;

        private void generateParent()
        {
            trailContainer = new GameObject();
            trailContainer.AddComponent<CompositeCollider2D>(); 
        }

        public override bool Use(Vector2 holderPosition, Vector2 holderDirection)
        {
            throw new NotImplementedException();

            /*if(!trailContainer)
            {
                generateParent();
            }

            Vector2 fillDirection;
            if (Mathf.Abs(holderDirection.x) > 0 && Mathf.Abs(holderDirection.y) > 0)
            {
                fillDirection = new Vector2(0f, holderDirection.x).normalized;
            }
            else
            {
                fillDirection = new Vector2(holderDirection.y, holderDirection.x);
            }

            for(int i = 0; i < _numberPerLine; i++)
            {
                Vector2 randomOffset = new Vector2(Random.Range(_minOffset.x, _maxOffset.x), Random.Range(_minOffset.x, _maxOffset.x));
                float randomSpacing = Random.Range(_minSpacing, _maxSpacing);
                Vector2 firstPosition = holderPosition - (randomOffset * holderDirection) + ((_numberPerLine / 2f) * randomSpacing * fillDirection);

                Vector2 newPosition = firstPosition + (i * randomSpacing * -fillDirection); ;

                GameObject projectile = GetProjectile();
                projectile.transform.parent = trailContainer.transform;
                trailContainer.transform.position = holderPosition;
                //We want to keep the Z-index of the original prefab
                projectile.transform.position = new Vector3(newPosition.x, newPosition.y, projectile.transform.position.z);
                projectile.transform.rotation = Quaternion.identity;

                projectile.SetActive(true);
            }*/

        }
    }
}