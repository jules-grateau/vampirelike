using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects.Items
{
    public class Power : MonoBehaviour
    {
        public static Power instance;
        void Start()
        {
            Power.instance = this;
        }
    }
}