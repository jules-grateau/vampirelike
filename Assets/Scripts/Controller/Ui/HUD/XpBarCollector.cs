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
using Assets.Scripts.Events.TypedEvents;

namespace Assets.Scripts.Controller.Ui
{
    public class XpBarCollector : MonoBehaviour
    {
        [SerializeField]
        GameEventFloat OnXpEvent;
        [SerializeField] 
        GameObject animatedSoulPrefab;
        [SerializeField] 
        GameObject target;
        [SerializeField] int maxSouls;
        Queue<GameObject> soulsQueue = new Queue<GameObject>();
        [SerializeField]
        float minAnimDuration;
        [SerializeField]
        float maxAnimDuration;
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
            GameObject xp;
            for (int i = 0; i < maxSouls; i++)
            {
                xp = Instantiate(animatedSoulPrefab);
                xp.transform.SetParent(transform);
                xp.SetActive(false);
                soulsQueue.Enqueue(xp);
            }
        }

        void Animate(Vector3 collectedXpPosition, int amount)
        {
            targetPosition = target.transform.position;
            for (int i = 0; i < amount; i++)
            {
                if (soulsQueue.Count > 0)
                {
                    GameObject xp = soulsQueue.Dequeue();
                    xp.transform.position = collectedXpPosition + new Vector3(Random.Range(-spread, spread), 0f, 0f);
                    xp.SetActive(true);
                    var orgDistance = Vector3.Distance(xp.transform.position, targetPosition);

                    float duration = Random.Range(minAnimDuration, maxAnimDuration);
                    Tweener tweener = xp.transform.DOMove(targetPosition, duration)
                    .SetEase(easeType);

                    tweener.OnUpdate(() =>
                    {
                        var newDistance = Vector3.Distance(xp.transform.position, target.transform.position);
                        if (newDistance > 0.1)
                        {
                            tweener.ChangeValues(xp.transform.position, target.transform.position, (newDistance / orgDistance) * duration);
                        }
                        else
                        {
                            tweener.Kill();
                            End(xp, amount);
                        }
                    });
                }
            }
        }

        private void End(GameObject xp, int amount)
        {
            target.GetComponent<ParticleSystem>().Play();
            xp.SetActive(false);
            soulsQueue.Enqueue(xp);
            OnXpEvent.Raise(amount);
        }

        public void AddXp(float xpAmount)
        {
            if (!_player)
            {
                _player = GameObject.FindGameObjectWithTag("Player");
            }
            Animate(_player.transform.position, Mathf.FloorToInt(xpAmount));
        }
    }
}