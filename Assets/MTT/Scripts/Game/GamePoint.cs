using Photon.Pun;
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
        [SerializeField] public PhotonView PhotonView;
        [SerializeField] private SpriteRenderer _spriteRenderer;

        private Vector2Int _index;
        private bool _block;
        private int _playerIndex;
        private bool _projectile;
        private int _projectileIndex;

        public Vector2Int Index => _index;
        public int PlayerIndex => _playerIndex;
        public bool Free => !_block && _playerIndex == -1;

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                GamePointInfo pointInfo = GetData();

                stream.SendNext(pointInfo.IndexX);
                stream.SendNext(pointInfo.IndexY);
                stream.SendNext(pointInfo.Block);
                stream.SendNext(pointInfo.PlayerIndex);
                stream.SendNext(pointInfo.Projectile);
                stream.SendNext(pointInfo.ProjectileIndex);
            }
            else
            {
                GamePointInfo pointInfo = new GamePointInfo
                {
                    IndexX = (int)stream.ReceiveNext(),
                    IndexY = (int)stream.ReceiveNext(),
                    Block = (bool)stream.ReceiveNext(),
                    PlayerIndex = (int)stream.ReceiveNext(),
                    Projectile = (bool)stream.ReceiveNext(),
                    ProjectileIndex = (int)stream.ReceiveNext()
                };

                GamePointInfo currentData = GetData();

                if (pointInfo.IndexX == currentData.IndexX &&
                    pointInfo.IndexY == currentData.IndexY &&
                    pointInfo.Block == currentData.Block &&
                    pointInfo.PlayerIndex == currentData.PlayerIndex &&
                    pointInfo.Projectile == currentData.Projectile &&
                    pointInfo.ProjectileIndex == currentData.ProjectileIndex)
                    return;

                SetData(pointInfo);
            }
        }

        private void UpdateView()
        {
            if (!_spriteRenderer || _spriteRenderer.color.a == 0)
                return;

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
                    default:
                        {
                            break;
                        }
                }
            }
            else
            {
                if (!_projectile && !_block)
                    _spriteRenderer.color = Color.white;                
            }
        }

        public void SetPlayer(int player)
        {
            _playerIndex = player;
            UpdateView();
        }

        public void Enable()
        {
            _spriteRenderer.color = new Color(1, 1, 1, 1);
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