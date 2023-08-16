using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Types;

public struct HitData
{
    public float damage {get;set;}
    public int instanceID { get; set; } 
    public Vector2 position { get; set; }
    public GameObject source { get; set; }

    public HitStatusEnum status { get; set; }

    public float time { get; set; }
    public object payload { get; set; }
}
