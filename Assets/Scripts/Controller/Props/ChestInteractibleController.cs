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
    public List<Loot> Loots;
    [SerializeField]
    Ease easeType;
    [SerializeField]
    float dropTime;
    [SerializeField]
    bool destroyAfterLoot = false;

    private Queue<GameObject> queue = new Queue<GameObject>();

    public void SetLoots(List<Loot> loots)
    {
        Loots = loots;
        foreach (Loot loot in Loots)
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

    public void SetLootQueue(Queue<GameObject> loots)
    {
        foreach (GameObject loot in loots)
        {
            loot.transform.position = gameObject.transform.position;
            loot.transform.SetParent(gameObject.transform);
            loot.SetActive(false);
            queue.Enqueue(loot);
        }
    }

    protected override void TriggerAnimation()
    {
        DropAnimation();
    }

    private void DropAnimation()
    {
        if (queue.Count <= 0)
        {
            SetIsUsable(false);
            if (destroyAfterLoot)
            {
                Destroy(gameObject);
            }
            return;
        }

        GameObject lootGO = queue.Dequeue();
        lootGO.transform.SetParent(null);
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
            DropAnimation();
        });
    }
}