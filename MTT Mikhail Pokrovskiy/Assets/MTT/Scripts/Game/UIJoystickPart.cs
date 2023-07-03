using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MTT
{
    internal class UIJoystickPart : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public delegate void ButtonPressHandler(ControlType control);
        public event ButtonPressHandler ButtonPress;

        private bool _pressed;

        [SerializeField] private ControlType _control;

        private float _timer;
        [SerializeField] private float _cooldown;

        public void OnPointerDown(PointerEventData eventData)
        {
            _timer = _cooldown;
            _pressed = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _pressed = false;
        }

        private void Update()
        {
            if (_pressed)
            {
                _timer += Time.deltaTime;
                if (_timer > _cooldown)
                {
                    ButtonPress?.Invoke(_control);
                    _timer = 0;
                }
            }
        }
    }
}