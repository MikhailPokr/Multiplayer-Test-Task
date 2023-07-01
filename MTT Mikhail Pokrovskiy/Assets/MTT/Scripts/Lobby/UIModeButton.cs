using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MTT
{
    [RequireComponent(typeof(EventTrigger))]
    internal class UIModeButton : MonoBehaviour, IPointerClickHandler
    {
        public bool CreateRoomButton;

        public delegate void ClickHandler(bool createButton);
        public event ClickHandler Click;
        public void OnPointerClick(PointerEventData eventData)
        {
            Click?.Invoke(CreateRoomButton);
        }
    }
}