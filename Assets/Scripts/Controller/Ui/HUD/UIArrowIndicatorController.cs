using Assets.Scripts.ScriptableObjects.Items;
using System.Collections;
using Assets.Scripts.Controller.Game;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Controller.Collectible;

public class UIArrowIndicatorController : MonoBehaviour
{
    private InteractibleController _interactible;
    private GameObject _player;
    private Image _image;

    public void Awake()
    {
        gameObject.SetActive(false); 
        _player = GameManager.GameState.Player;
        _image = gameObject.GetComponent<Image>();
    }

    public void setCollectible(KeyCollectible keyCollectible)
    {
        _interactible = keyCollectible.Interactible;
        _image.color = keyCollectible.Color;
        gameObject.SetActive(true);
    }

    private void Update()
    {
        if (!_player) return;
        if (!_interactible) return;
        float ratio = Vector2.Distance(_player.transform.position, _interactible.gameObject.transform.position) / 10f;

        _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, ratio);
        Vector3 direction = _interactible.gameObject.transform.position - _player.transform.position;
        gameObject.transform.position = _player.transform.position + direction.normalized * 2f;
        gameObject.transform.rotation = Quaternion.FromToRotation(-Vector3.right, direction);
    }
}
