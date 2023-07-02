using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MTT
{
    internal class UIJoystickPart : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private ControlType _control;
        public void OnPointerClick(PointerEventData eventData)
        {
            Singleton<Controls>.instance.Test(_control);
        }
    }
}