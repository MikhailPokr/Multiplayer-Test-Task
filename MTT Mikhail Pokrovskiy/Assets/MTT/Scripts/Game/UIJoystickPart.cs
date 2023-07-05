using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MTT
{
    internal class UIJoystickPart : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private VirtualJoystick _joystick;

        private bool _pressed;

        [SerializeField] private ControlType _control;

        private float _timer;

        public void OnPointerDown(PointerEventData eventData)
        {
            _pressed = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _pressed = false;
        }

        private void Update()
        {
            _timer += Time.deltaTime;
            if (_pressed)
            {
                if (_timer > _joystick.Cooldown)
                {
                    _joystick.Press(_control);
                    _timer = 0;
                }
            }
        }
    }
}