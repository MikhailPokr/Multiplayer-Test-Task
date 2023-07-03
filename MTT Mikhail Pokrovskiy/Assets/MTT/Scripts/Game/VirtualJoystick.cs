using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace MTT
{
    internal class VirtualJoystick : MonoBehaviour
    {
        [SerializeField] List<UIJoystickPart> _parts;

        private void Awake()
        {
            foreach (UIJoystickPart part in _parts) 
            {
                part.ButtonPress += OnButtonPress;
            }
        }

        private void OnButtonPress(ControlType control)
        {
            Singleton<Controls>.instance.TakeAction(control);
        }
    }
}