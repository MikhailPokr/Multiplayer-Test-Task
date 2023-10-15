using Photon.Pun;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace MTT
{
    [Singleton]
    internal class GamePointsManager : MonoBehaviourPunCallbacks, ISingleton
    {
        public Color Color0 = Color.white;
        public Color Color1 = Color.white;
        public Color Color2 = Color.white;
        public Color Color3 = Color.white;



        private GamePoint _lastPoint;

        private List<GamePoint>[] _gamePoints = new List<GamePoint>[4];

        private Vector2Int _maxSize;

        private Player _player;

        private bool _findPoints;

        private bool _gameStarted = false;


        private void FixedUpdate()
        {
            if (_gamePoints[0] == null && PhotonNetwork.LocalPlayer.ActorNumber == 1)
                return;

            if (_findPoints)
            {
                GamePoint[] allPoints = FindObjectsOfType<GamePoint>();
                if (allPoints.Length > _maxSize.x * _maxSize.y)
                {
                    _findPoints = false;

                    SetPoints(allPoints, _maxSize);
                }
            }

            if (_player == null)
                return;

            for (int i = 0; i < _gamePoints[0].Count; i++)
            {
                int playerIndex = -1;
                for (int j = 0; j < 4; j++)
                {
                    if (_gamePoints[j] == null || _gamePoints[j][i] == null)
                        continue;

                    if (j == _player.Index)
                    {
                        if (_gamePoints[j][i] == _lastPoint)
                        {
                            playerIndex = j;
                        }
                    }
                    else
                    {
                        if (_gamePoints[j][i].PlayerIndex == j)
                        {
                            playerIndex = j;
                        }
                    }
                }
                for (int j = 0; j < 4; j++)
                {
                    if (_gamePoints[j] == null || _gamePoints[j][i] == null)
                        continue;
                    _gamePoints[j][i].SetPlayer(playerIndex);
                }
            }
        }


        public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
        {
            _findPoints = true;            
        }


        public void SetPoints(GamePoint[] allPoints, Vector2Int size)
        {
            for (int j = 0; j < 4; j++)
            {
                if (_gamePoints[j] != null)
                    continue;
                List<GamePoint> newPoints = allPoints.ToList().FindAll(x => x.PhotonView.Controller.ActorNumber - 1 == j);
                if (newPoints.Count > 0)
                {
                    SetNewPoints(newPoints, size);
                }
            }
        }
        public void SetPoints(Vector2Int size)
        {
            _maxSize = size;
            _findPoints = true;
        }

        public void SetNewPoints(List<GamePoint> points, Vector2Int size)
        {
            List<GamePoint> newList = new List<GamePoint>();
            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    newList.Add(points.Find(i => i.Index == new Vector2Int(x, y)));
                }
            }

            _gamePoints[_gamePoints.ToList().FindIndex(x => x == null)] = newList;

            _maxSize = size;
        }

        public void MoveToken(Player player, Vector2Int deltaPos)
        {
            if (!_gameStarted)
                return;
            GamePoint point = _gamePoints[player.Index].Find(x => x.PlayerIndex == player.Index);
            Vector2Int newPos = point.Index + deltaPos;
            GamePoint newPoint = _gamePoints[player.Index].Find(x => x.Index == newPos);

            if (newPoint == null || !newPoint.Free )
                return;

            _lastPoint = newPoint;
        }

        public void CreateToken(Player player)
        {
            _player = player;

            int x = player.Index == 0 || player.Index == 2 ? 0 : _maxSize.x - 1;

            int y = player.Index == 0 || player.Index == 1 ? 0 : _maxSize.y - 1;

            GamePoint point = _gamePoints[player.Index].Find(i => i.Index == new Vector2Int(x, y));
            point.SetPlayer(player.Index);

            print($"Создан токен персонажа {player.Index} в точке {point.Index.x}x{point.Index.y}");
            _lastPoint = point;
        }

        public bool CheckTokens()
        {
            int players = PhotonNetwork.PlayerList.Length;
            for (int i = 0; i < players; i++)
            {
                if (_gamePoints[i] == null)
                    return false;
                if (_gamePoints[i].Contains(null))
                {
                    return false;
                }
                try
                {
                    if (_gamePoints[i].Find(x => x.PlayerIndex != -1) == null)
                    {
                        return false;
                    }
                }
                catch
                {

                }
                
            }
            return true;
        }

        public void StartGame()
        {
            _gameStarted = true;
        }
    }
}