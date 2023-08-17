using System.Collections;
using UnityEngine;
using Assets.Scripts.ScriptableObjects.Status;

namespace Assets.Scripts.Controller.Enemies.Interface
{
    public class IEnemyDamage : MonoBehaviour
    {
        public float Damage { get; set; }

        public StatusSO Status { get; set; }
    }
}