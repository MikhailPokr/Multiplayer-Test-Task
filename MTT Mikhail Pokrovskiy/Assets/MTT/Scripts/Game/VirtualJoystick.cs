using Unity.VisualScripting;
using UnityEngine;

namespace MTT
{
    internal class VirtualJoystick : MonoBehaviour
    {
        public float Cooldown;

        public void Press(ControlType control)
        {
            Singleton<Controls>.instance.TakeAction(control);
        }
    }
}