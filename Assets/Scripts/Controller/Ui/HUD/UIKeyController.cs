using Assets.Scripts.ScriptableObjects.Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Controller.Collectible;

public class UIKeyController : MonoBehaviour
{
    private GameObject _keyUIPrefab;
    private GameObject _arrowUIPrefab;
    private Dictionary<KeyCollectible, (GameObject, GameObject)> _map;

    public void Start()
    {
        _keyUIPrefab = (GameObject) Resources.Load("Prefabs/UI/Key");
        _arrowUIPrefab = (GameObject)Resources.Load("Prefabs/UI/Arrow");
        _map = new Dictionary<KeyCollectible, (GameObject, GameObject)>();
    }

    public void AddKey(CollectibleItem collectible)
    {
        KeyCollectible keyCollectible = (KeyCollectible)collectible;

        GameObject key = Instantiate(_keyUIPrefab, gameObject.transform);
        key.GetComponent<Image>().color = keyCollectible.Color;

        GameObject arrow = Instantiate(_arrowUIPrefab, gameObject.transform.parent.parent);
        arrow.GetComponent<UIArrowIndicatorController>().setCollectible(keyCollectible);
        _map.Add(keyCollectible, (arrow, key));
    }

    public void RemoveKey(CollectibleItem collectible)
    {
        KeyCollectible keyCollectible = (KeyCollectible)collectible;
        var (arrow, key) = _map[keyCollectible];
        _map.Remove(keyCollectible);
        Destroy(arrow);
        Destroy(key);
    }
}
