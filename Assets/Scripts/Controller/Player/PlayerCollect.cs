using Assets.Scripts.Events;
using Assets.Scripts.Variables;
using Assets.Scripts.Variables.Constants;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Controller.Player
{
    public class PlayerCollect : MonoBehaviour
    {
        [SerializeField]
        private FloatVariable _radius;

        public float getRadius()
        {
            return _radius.value;
        }
    }
}