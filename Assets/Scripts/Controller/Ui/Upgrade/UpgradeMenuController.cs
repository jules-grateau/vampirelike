using Assets.Scripts.Events;
using Assets.Scripts.ScriptableObjects;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Controller.Ui.Upgrade
{
    public class UpgradeMenuController : MonoBehaviour
    {

        [SerializeField]
        GameEvent _unpauseEvent;
        [SerializeField]
        string _upgradeMenuSceneName;

        public void OnSelectUpgrade(UpgradeSO upgrade)
        {
            SceneManager.UnloadSceneAsync(_upgradeMenuSceneName);
            _unpauseEvent.Raise();
        }
    }
}