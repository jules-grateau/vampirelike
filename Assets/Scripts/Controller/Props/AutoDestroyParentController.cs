using UnityEngine;

public class AutoDestroyParentController : MonoBehaviour
{
    public void DestroyParent() { 
        Destroy(gameObject.transform.parent.gameObject); 
    }
}