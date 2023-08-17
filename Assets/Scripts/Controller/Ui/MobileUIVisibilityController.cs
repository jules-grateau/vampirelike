using UnityEngine;
public class MobileUIVisibilityController : MonoBehaviour
{

    private void Awake()
    {
        gameObject.SetActive(UnityEngine.Device.Application.isMobilePlatform);

    }
}
