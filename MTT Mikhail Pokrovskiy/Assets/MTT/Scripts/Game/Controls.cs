using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace MTT
{
    internal enum ControlType
    {
        Left,
        Right,
        Up,
        Down,
        Shoot
    }

    [Singleton]
    internal class Controls : MonoBehaviourPunCallbacks, ISingleton
    {
        public delegate void ControlHendler(Photon.Realtime.Player player, ControlType control);
        public event ControlHendler Control;

        public void Test(ControlType control)
        {
            Control?.Invoke(PhotonNetwork.LocalPlayer, control);
        }
    }
}