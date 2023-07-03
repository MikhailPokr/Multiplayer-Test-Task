using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace MTT
{
    internal struct GamePointInfo
    {
        public int IndexX;
        public int IndexY;
        public bool Block;
        public int PlayerIndex;
        public bool Projectile;
        public int ProjectileIndex;
    }
    internal class GamePoint : MonoBehaviour, IPunObservable
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;

        private Vector2Int _index;
        private bool _block;
        private int _playerIndex;
        private bool _projectile;
        private int _projectileIndex;

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            print("Синхронизация");
            if (stream.IsWriting)
            {
                stream.SendNext(GetData());
            }
            else
            {
                SetData((GamePointInfo)stream.ReceiveNext());
            }
        }

        private void UpdateView()
        {
            if (_block)
            {
                _spriteRenderer.color = Color.black;
            }
            if (_projectile)
            {
                switch (_projectileIndex)
                {
                    case 0:
                        {
                            _spriteRenderer.color = Singleton<GamePointsManager>.instance.Color0 / 2;
                            break;
                        }
                    case 1:
                        {
                            _spriteRenderer.color = Singleton<GamePointsManager>.instance.Color1 / 2;
                            break;
                        }
                    case 2:
                        {
                            _spriteRenderer.color = Singleton<GamePointsManager>.instance.Color2 / 2;
                            break;
                        }
                    case 3:
                        {
                            _spriteRenderer.color = Singleton<GamePointsManager>.instance.Color3 / 2;
                            break;
                        }
                }
            }
            if (_playerIndex != -1)
            {
                switch (_playerIndex)
                {
                    case 0:
                        {
                            _spriteRenderer.color = Singleton<GamePointsManager>.instance.Color0;
                            break;
                        }
                    case 1:
                        {
                            _spriteRenderer.color = Singleton<GamePointsManager>.instance.Color1;
                            break;
                        }
                    case 2:
                        {
                            _spriteRenderer.color = Singleton<GamePointsManager>.instance.Color2;
                            break;
                        }
                    case 3:
                        {
                            _spriteRenderer.color = Singleton<GamePointsManager>.instance.Color3;
                            break;
                        }
                }
            }
        }

        public void SetPlayer(int player)
        {
            _playerIndex = player;
            UpdateView();
        }

        public GamePointInfo GetData() => new GamePointInfo()
        {
            IndexX = _index.x,
            IndexY = _index.y,
            Block = _block,
            PlayerIndex = _playerIndex,
            Projectile = _projectile,
            ProjectileIndex = _projectileIndex
        };


        public void SetData(GamePointInfo info)
        {
            _index = new Vector2Int(info.IndexX, info.IndexY);
            _block = info.Block;
            _playerIndex = info.PlayerIndex;
            _projectile = info.Projectile;
            _projectileIndex = info.ProjectileIndex;
            UpdateView();
        }
    }
}