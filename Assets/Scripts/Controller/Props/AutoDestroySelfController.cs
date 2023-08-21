using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroySelfController : MonoBehaviour
{
    public void DestroySelf() { 
        Destroy(gameObject); 
    }
}