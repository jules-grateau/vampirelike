using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Assets.Scripts.ScriptableObjects.Items;
using DG.Tweening;

[Serializable]
public class Loot
{
    public BaseCollectibleSO Item;
    public int Amount;
}

public class ChestInteractibleController : InteractibleController
{
    [SerializeField]
    private List<Loot> _loots;
    [SerializeField]
    Ease easeType;
    [SerializeField]
    float dropTime;

    private Queue<GameObject> queue = new Queue<GameObject>();

    protected override void PrepareLoot()
    {
        foreach (Loot loot in _loots)
        {
            for (int i = 0; i < loot.Amount; i++)
            {
                GameObject lootGO = loot.Item.GetGameObject(gameObject.transform.position);
                lootGO.transform.SetParent(gameObject.transform);
                lootGO.SetActive(false);
                queue.Enqueue(lootGO);
            }
        }
    }

    protected override void TriggerAnimation()
    {
        DropAnimation();
    }

    private void DropAnimation()
    {
        GameObject lootGO = queue.Dequeue();
        Collider2D lootColider = lootGO.GetComponent<Collider2D>();
        lootColider.enabled = false;
        Vector2 offset = UnityEngine.Random.insideUnitCircle;
        Vector3 toPositon = gameObject.transform.position + new Vector3(offset.x, offset.y, 0f);
        lootGO.SetActive(true);
        Tweener tweener = lootGO.transform.DOMove(toPositon, dropTime / (queue.Count + 1))
            .SetEase(easeType);

        tweener.OnComplete(() =>
        {
            lootColider.enabled = true;
            if (queue.Count > 0)
            {
                DropAnimation();
            }
        });
    }
}