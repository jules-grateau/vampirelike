using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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

    public void OnDropCollectible()
    {
        SeekCollectibleController seekCollectible = gameObject.GetComponent<SeekCollectibleController>();
        if (seekCollectible)
        {
            GameObject stash = Instantiate(Resources.Load<GameObject>("Prefabs/Props/Persistant/stash_1"), gameObject.transform.position, Quaternion.identity);
            ChestInteractibleController chestInteractible = stash.GetComponent<ChestInteractibleController>();
            chestInteractible.SetLootQueue(seekCollectible.Loots);
        }
        else
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
}
