using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace MTT
{
    [Singleton]
    internal class GamePointsManager : MonoBehaviourPunCallbacks, ISingleton
    {
        [SerializeField] private Player _playerPrefab;
        public Color Color0 = Color.white;
        public Color Color1 = Color.white;
        public Color Color2 = Color.white;
        public Color Color3 = Color.white;

        private List<GamePoint> _gamePoints;
        private Vector2Int _maxSize; 

        
        public void SetPoints(List<GamePoint> points, Vector2Int size)
        {
            _gamePoints = points;
            _maxSize = size;

            Photon.Realtime.Player[] players = PhotonNetwork.PlayerList;
            CreatePlayer(players[0]);
        }

        private void CreatePlayer(Photon.Realtime.Player photonPlayer)
        {
            Player player = PhotonNetwork.Instantiate(_playerPrefab.name, Vector2.zero, Quaternion.identity).GetComponent<Player>();

            player.PhotonPlayer = photonPlayer;

            CreateToken(player);
        }

        public void CreateToken(Player player)
        {
            int index = PhotonNetwork.PlayerList.ToList().FindLastIndex(x => x.ActorNumber == player.PhotonPlayer.ActorNumber);
            print(index);

            int x = index == 0 || index == 2 ? 0 : _maxSize.x - 1;

            int y = index == 0 || index == 1 ? 0 : _maxSize.y - 1;

            GamePoint point = _gamePoints.Find(i => i.GetData().IndexX == x && i.GetData().IndexY == y);
            point.SetPlayer(index);
        }
    }
}