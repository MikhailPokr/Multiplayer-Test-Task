using Photon.Pun;
using System;
using Unity.VisualScripting;
using UnityEngine;

namespace MTT
{
    [RequireComponent(typeof(PhotonView))]
    internal class Player : MonoBehaviourPunCallbacks
    {
        public PhotonView PhotonView;
        public Photon.Realtime.Player PhotonPlayer;

        private void Start()
        {
            Singleton<Controls>.instance.Control += OnControl;
        }

        private void OnControl(Photon.Realtime.Player player, ControlType control)
        {
            switch (control)
            {
                case ControlType.Up:
                    {
                        break;
                    }
                case ControlType.Down:
                    {
                        break;
                    }
                case ControlType.Left:
                    {
                        break;
                    }
                case ControlType.Right:
                    {
                        break;
                    }
                case ControlType.Shoot:
                    {
                        break;
                    }
            }
        }
    }
}