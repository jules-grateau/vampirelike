using Assets.Scripts.ScriptableObjects.Items;
using System.Collections;
using Assets.Scripts.Controller.Game;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Controller.Collectible;

public class UIArrowIndicatorController : MonoBehaviour
{
    private float _radius = 0;
    private InteractibleController _interactible;
    private GameObject _player;

    public void Awake()
    {
        _radius = Mathf.Abs((Camera.main.transform.position.x - Camera.main.orthographicSize * Screen.width / Screen.height) / 1.75f);
        gameObject.SetActive(false); 
        _player = GameManager.GameState.Player;
    }

    public void setCollectible(KeyCollectible keyCollectible)
    {
        _interactible = keyCollectible.Interactible;
        gameObject.GetComponent<Image>().color = keyCollectible.Color;
        gameObject.SetActive(true);
    }

    private void Update()
    {
        if (!_player) return;
        if (!_interactible) return;
        bool isTooClose = Vector2.Distance(_player.transform.position, _interactible.gameObject.transform.position) < _radius;

        gameObject.SetActive(!isTooClose);
        Vector3 direction = _interactible.gameObject.transform.position - _player.transform.position;
        gameObject.transform.position = _player.transform.position + direction.normalized * _radius;
        gameObject.transform.rotation = Quaternion.FromToRotation(-Vector3.right, direction);
    }
}
