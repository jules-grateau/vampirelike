using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Controller.Ui.LanguageSelection
{
    public class OptionHide : MonoBehaviour
    {

        public void Hide(bool isOn)
        {
            gameObject.SetActive(!isOn);
        }
    }
}