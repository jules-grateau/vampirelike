using UnityEditor;
using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Types;
using System.Linq;
using Assets.Scripts.ScriptableObjects.Status;

namespace Assets.Scripts.Controller.Enemies
{
    public class EnemyHealth : BaseHealth
    {

        [SerializeField]
        public AudioClip deathAudioClip;

        protected override void triggerBeforeDestroy()
        {
            AudioSource.PlayClipAtPoint(deathAudioClip, transform.position, 1);
        }
    }
}