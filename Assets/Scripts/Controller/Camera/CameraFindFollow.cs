using Cinemachine;
using UnityEngine;

namespace Assets.Scripts.Controller
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class CameraFindFollow : MonoBehaviour
    {
        CinemachineVirtualCamera _virtualCamera;
        [SerializeField]
        string _followTag;

        // Use this for initialization
        void Start()
        {
            _virtualCamera = GetComponent<CinemachineVirtualCamera>();
            GameObject player = GameObject.FindGameObjectWithTag(_followTag);
            _virtualCamera.Follow = player.transform;
        }
    }
}