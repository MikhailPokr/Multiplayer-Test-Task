using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Unity.VisualScripting;

namespace MTT
{
    [RequireComponent(typeof(PhotonView))]
    internal class Player : MonoBehaviour
    {
        [SerializeField] private PhotonView _view;

        private void Start()
        {
            Singleton<Controls>.instance.Control += OnControl;
        }

        private void OnControl(Photon.Realtime.Player player, ControlType control)
        {
            if (player == _view.Controller)
            {
                switch (control)
                {
                    case ControlType.Up:
                        {
                            transform.position = new Vector2(transform.position.x, transform.position.y + 1);
                            break;
                        }
                    case ControlType.Down:
                        {
                            transform.position = new Vector2(transform.position.x, transform.position.y - 1);
                            break;
                        }
                    case ControlType.Left:
                        {
                            transform.position = new Vector2(transform.position.x - 1, transform.position.y);
                            break;
                        }
                    case ControlType.Right:
                        {
                            transform.position = new Vector2(transform.position.x + 1, transform.position.y);
                            break;
                        }

                }

                
            }
        }
    }
}


