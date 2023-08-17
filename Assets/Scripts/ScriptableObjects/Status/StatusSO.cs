using Assets.Scripts.Types;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects.Status
{
    [CreateAssetMenu(fileName = "Status", menuName = "Status", order = 9)]
    public class StatusSO : ScriptableObject
    {
        [SerializeField]
        public Sprite sprite;
        [SerializeField]
        public Color color;
        [SerializeField]
        public bool isDoT;
        [SerializeField]
        [DrawIf("isDoT", true, ComparisonType.Equals, DisablingType.DontDraw)]
        public float doTTime;
        [DrawIf("isDoT", true, ComparisonType.Equals, DisablingType.DontDraw)]
        public float doTRatio;
        [SerializeField]
        public bool canBump;
        [SerializeField]
        [DrawIf("canBump", true, ComparisonType.Equals, DisablingType.DontDraw)]
        public Vector2 bumpForce;
        [SerializeField]
        public bool isStun;
        [SerializeField]
        [DrawIf("isStun", true, ComparisonType.Equals, DisablingType.DontDraw)]
        public float stunTime;
        [SerializeField]
        public bool isSlow;
        [SerializeField]
        [DrawIf("isSlow", true, ComparisonType.Equals, DisablingType.DontDraw)]
        public float slowValue;
        [SerializeField]
        [DrawIf("isSlow", true, ComparisonType.Equals, DisablingType.DontDraw)]
        public float slowTime;
    }
}