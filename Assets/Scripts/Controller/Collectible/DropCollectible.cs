using System;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DropCollectible : MonoBehaviour
{
    [SerializeField]
    private Dictionary<Func<Vector3, GameObject>, int> _collectibles = new Dictionary<Func<Vector3, GameObject>, int>();

    public void addCollectibleFunction(Func<Vector3, GameObject> buildFunction)
    {
        addCollectibleFunction(buildFunction, 1);
    }
    public void addCollectibleFunction(Func<Vector3, GameObject> buildFunction, int amount)
    {
        _collectibles.Add(buildFunction, amount);
    }

    public void onDestroy()
    {
        foreach (var _collectible in _collectibles)
        {
            for (int i = 0; i < _collectible.Value; i++)
            {
                Vector2 offset = UnityEngine.Random.insideUnitCircle * 0.5f;
                _collectible.Key(gameObject.transform.position + new Vector3(offset.x, offset.y, 0));
            }
        }
    }
}
