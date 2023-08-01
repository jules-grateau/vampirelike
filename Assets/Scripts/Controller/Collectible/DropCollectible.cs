using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropCollectible : MonoBehaviour
{
    [SerializeField]
    public GameObject Collectible { get; set; }
    [SerializeField]
    private int _amount = 1;

    public void onDestroy()
    {
        for (int i = 0; i < _amount; i++)
        {
            Vector2 offset = UnityEngine.Random.insideUnitCircle * 0.5f;
            Collectible.transform.position = gameObject.transform.position + new Vector3(offset.x, offset.y, 0);
            Collectible.SetActive(true);
        }
    }
}
