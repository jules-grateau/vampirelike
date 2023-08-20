using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Controller.Ui
{
    public class DefaultInterfaceElementOpener : OpenInterfaceElement
    {
        private void Start()
        {
            OnOpenInterface();
        }
    }
}