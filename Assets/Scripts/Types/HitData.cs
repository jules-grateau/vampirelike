using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Types;
using Assets.Scripts.ScriptableObjects.Status;
using Assets.Scripts.ScriptableObjects.Items;

public struct HitData
{
    public float damage {get;set;}
    public int instanceID { get; set; } 
    public Vector2 position { get; set; }
    public GameObject source { get; set; }
    public StatusSO status { get; set; }

    public WeaponSO weapon { get; set; }
}
