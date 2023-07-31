using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroyParticleController : MonoBehaviour
{
    public void OnParticleSystemStopped()
    {
        Destroy(gameObject);
    }
}