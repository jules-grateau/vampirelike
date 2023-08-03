using System;
using System.Collections.Generic;
using UnityEngine;

public class DropCollectible : MonoBehaviour
{
    [SerializeField]
    private Func<Vector3, GameObject> _buildCollectible { get; set; }
    [SerializeField]
    private int _amount = 1;

    public void setCollectibleFunction(Func<Vector3, GameObject> buildFunction)
    {
        _buildCollectible = buildFunction;
    }

    public void onDestroy()
    {
        for (int i = 0; i < _amount; i++)
        {
            Vector2 offset = UnityEngine.Random.insideUnitCircle * 0.5f;
            _buildCollectible(gameObject.transform.position + new Vector3(offset.x, offset.y, 0));
        }
    }
}
