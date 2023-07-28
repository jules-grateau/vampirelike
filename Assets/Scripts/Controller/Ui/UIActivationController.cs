using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Controller.Ui
{
    public class UIActivationController : MonoBehaviour
    {
        public void DisableAllChildren()
        {
            foreach(Transform child in transform) 
            {
                child.gameObject.SetActive(false);
            }
        }

        public void EnableAllChildren()
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(true);
            }
        }
    }
}