using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropCollectible : MonoBehaviour
{
    [SerializeField]
    private GameObject _collectible;
    [SerializeField]
    private int _amount = 1;

    // Start is called before the first frame update
    void Awake()
    {
        this._collectible = Resources.Load<GameObject>("Prefabs/Collectibles/soul_1");
    }

    public void onDestroy()
    {
        for (int i = 0; i < _amount; i++)
        {
            Vector2 offset = UnityEngine.Random.insideUnitCircle * 0.5f;
            GameObject s = Instantiate(this._collectible);
            s.transform.position = gameObject.transform.position + new Vector3(offset.x, offset.y, 0);
        }
    }
}
