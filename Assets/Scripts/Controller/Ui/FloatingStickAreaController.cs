using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.OnScreen;

namespace Assets.Scripts.Controller.Ui
{
    public class FloatingStickAreaController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        [SerializeField]
        GameObject _joystick;

        OnScreenStick _screenStick;
        RectTransform _mainRect;
        RectTransform _joystickRect;

        private void Awake()
        {
            _screenStick = _joystick.GetComponentInChildren<OnScreenStick>();
            _mainRect = GetComponent<RectTransform>();
            _joystickRect = _joystick.GetComponent<RectTransform>();

        }

        public void OnDrag(PointerEventData eventData)
        {
            ExecuteEvents.dragHandler(_screenStick, eventData);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Vector2 localPosition;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(_mainRect, eventData.pressPosition, Camera.main, out localPosition);

            _joystickRect.localPosition = localPosition;

            ExecuteEvents.pointerDownHandler(_joystick.GetComponentInChildren<OnScreenStick>(), eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            ExecuteEvents.pointerUpHandler(_screenStick, eventData);
        }
    }
}