using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Controller.Ui
{
    public class ActivableUI : MonoBehaviour
    {

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                gameObject.SetActive(false);
            }
        }
        public void Activate()
        {
            gameObject.SetActive(true);
        }
    }
}