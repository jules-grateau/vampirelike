using Cinemachine;
using UnityEngine;
using Assets.Scripts.Controller.Game;

namespace Assets.Scripts.Controller
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class CameraFindFollow : MonoBehaviour
    {
        CinemachineVirtualCamera _virtualCamera;

        // Use this for initialization
        void Start()
        {
            _virtualCamera = GetComponent<CinemachineVirtualCamera>();
            GameObject player = GameManager.GameState.Player;
            _virtualCamera.Follow = player.transform;
        }
    }
}