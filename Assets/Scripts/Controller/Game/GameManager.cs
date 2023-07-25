using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Controller.Game
{
    public class GameManager : MonoBehaviour
    {
        public void OnPause()
        {
            Time.timeScale = 0;
        }

        public void OnUnpause()
        {
            Time.timeScale = 1;
        }
    }
}