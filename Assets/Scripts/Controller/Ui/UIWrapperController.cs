using Assets.Scripts.Types;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Controller.Ui
{
    public class UIWrapperController : MonoBehaviour
    {
        [SerializeField]
        InterfaceElement _interfaceElement;

        public void OnOpenInterfaceElement(InterfaceElement interfaceElement)
        {
            if (interfaceElement != _interfaceElement)
            {
                transform.GetChild(0).gameObject.SetActive(false);
                return;
            };


            transform.GetChild(0).gameObject.SetActive(true);
        }
    }
}