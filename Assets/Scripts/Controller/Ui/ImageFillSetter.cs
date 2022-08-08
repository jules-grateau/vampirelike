using Assets.Scripts.Variables;
using Assets.Scripts.Variables.Constants;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Controller.Ui
{
    [RequireComponent(typeof(Image))]
    public class ImageFillSetter : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Value to use as the current ")]
        private FloatVariable _variable;

        [SerializeField]
        [Tooltip("Min value that Variable to have no fill on Image.")]
        private float _min;

        [SerializeField]
        [Tooltip("Max value that Variable can be to fill Image.")]
        private FloatVariable _max;

        [Tooltip("Image to set the fill amount on.")]
        private Image _image;

        private void Start()
        {
            _image = GetComponent<Image>();
        }

        private void Update()
        {
            _image.fillAmount = Mathf.Clamp01(
                Mathf.InverseLerp(_min, _max.value, _variable.value));
        }
    }
}