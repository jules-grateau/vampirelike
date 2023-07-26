using UnityEngine;

namespace Assets.Scripts.ScriptableObjects.Game
{
    public class GameDataSO : ScriptableObject
    {
        public GameObject PlayerPrefab {
            get { return _playerPrefab; }
            set { _playerPrefab = value; }
        }

        [SerializeField]
        GameObject _playerPrefab;
    }
}