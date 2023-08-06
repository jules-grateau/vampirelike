using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DropCollectible : MonoBehaviour
{
    private Func<Vector3, GameObject> _createFunction;
    [SerializeField]
    private int _amount = 1;

    public void setCollectibleFunction(Func<Vector3, GameObject> createFunction)
    {
        _createFunction = createFunction;
    }

    public void onDestroy()
    {
        for (int i = 0; i < _amount; i++)
        {
            Vector2 offset = UnityEngine.Random.insideUnitCircle * 0.5f;
            _createFunction(gameObject.transform.position + new Vector3(offset.x, offset.y, 0));
        }
    }
}
