using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Controller.Ui
{
    public class UIActivationController : MonoBehaviour
    {
        public void DisableAllChildren()
        {
            Debug.Log("Disable all");
            foreach(Transform child in transform) 
            {
                child.gameObject.SetActive(false);
            }
        }

        public void EnableAllChildren()
        {
            Debug.Log("Enable all");
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(true);
            }
        }
    }
}