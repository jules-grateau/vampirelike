using System.Collections;
using UnityEngine;

namespace Assets.Types
{
    public class InputManager
    {
        static PlayerInputs _instance;

        public static PlayerInputs GetInstance()
        {
            if (_instance == null) _instance = new PlayerInputs();

            return _instance;
        }
    }
}