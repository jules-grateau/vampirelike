using UnityEngine;
public class MobileUIVisibilityController : MonoBehaviour
{
    private void OnEnable()
    {
        gameObject.SetActive(UnityEngine.Device.Application.isMobilePlatform);
    }
}
