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
        public int Index;

        private GamePointsManager _manager;

        public override void OnEnable()
        {
            base.OnEnable();

            Singleton<Controls>.instance.Control += OnControl;
            _manager = Singleton<GamePointsManager>.instance;

            Index = PhotonNetwork.LocalPlayer.ActorNumber - 1;
        }

        private void OnControl(Photon.Realtime.Player player, ControlType control)
        {
            if (PhotonPlayer == player)
            {
                switch (control)
                {
                    case ControlType.Up:
                        {
                            _manager.MoveToken(this, new Vector2Int(0, -1));
                            break;
                        }
                    case ControlType.Down:
                        {
                            _manager.MoveToken(this, new Vector2Int(0, 1));
                            break;
                        }
                    case ControlType.Left:
                        {
                            _manager.MoveToken(this, new Vector2Int(-1, 0));
                            break;
                        }
                    case ControlType.Right:
                        {
                            _manager.MoveToken(this, new Vector2Int(1, 0));
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
}