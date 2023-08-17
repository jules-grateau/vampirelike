using Assets.Scripts.Controller.Player;
using Assets.Scripts.ScriptableObjects.Characters;
using Assets.Scripts.Types;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;
using Assets.Scripts.Controller.Collectible;

namespace Assets.Scripts.Controller.Ui
{
    public class XpBarCollector : MonoBehaviour
    {
        [SerializeField] 
        GameObject animatedSoulPrefab;
        [SerializeField] 
        Transform target;
        [SerializeField] int maxSouls;
        Queue<GameObject> soulsQueue = new Queue<GameObject>();
        [SerializeField]
        [Range(0.5f, 0.9f)] float minAnimDuration;
        [SerializeField]
        [Range(0.9f, 2f)] float maxAnimDuration;
        [SerializeField]
        Ease easeType;
        [SerializeField]
        float spread;

        GameObject _player;
        Vector3 targetPosition;

        void Awake()
        {
            PrepareSouls();
        }

        void PrepareSouls()
        {
            GameObject coin;
            for (int i = 0; i < maxSouls; i++)
            {
                coin = Instantiate(animatedSoulPrefab);
                coin.transform.parent = transform;
                coin.SetActive(false);
                soulsQueue.Enqueue(coin);
            }
        }

        void Animate(Vector3 collectedCoinPosition, int amount)
        {
            targetPosition = Camera.main.ScreenToWorldPoint(target.position);
            for (int i = 0; i < amount; i++)
            {
                if (soulsQueue.Count > 0)
                {
                    GameObject coin = soulsQueue.Dequeue();
                    coin.SetActive(true);
                    coin.transform.position = collectedCoinPosition + new Vector3(Random.Range(-spread, spread), 0f, 0f);
                    var orgDistance = Vector3.Distance(coin.transform.position, targetPosition);

                    //animate coin to target position
                    float duration = Random.Range(minAnimDuration, maxAnimDuration);
                    Tweener tweener = coin.transform.DOMove(targetPosition, duration)
                    .SetEase(easeType)
                    .OnComplete(() => {
                        coin.SetActive(false);
                            soulsQueue.Enqueue(coin);
                        }
                    );

                    tweener.OnUpdate(() => { 
                        targetPosition = Camera.main.ScreenToWorldPoint(target.position);
                        tweener.ChangeEndValue(targetPosition);
                    });
                }
            }
        }

        public void AddCoins(float xpAmount)
        {
            if (!_player)
            {
                _player = GameObject.FindGameObjectWithTag("Player");
            }
            Animate(_player.transform.position, Mathf.FloorToInt(xpAmount));
        }
    }
}