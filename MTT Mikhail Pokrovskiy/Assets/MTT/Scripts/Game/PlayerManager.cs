using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace MTT
{
    [Singleton]
    public class PlayerManager : MonoBehaviourPunCallbacks, ISingleton
    {
        
        [SerializeField] private Player _playerPrefab;

        public bool IsGameReady(bool owner)
        {
            Photon.Realtime.Player[] list = PhotonNetwork.PlayerList;
            if (list.Length >= 2 && owner)
            {
                return true;
            }
            return false;
        }

        public void StartGame()
        {
            /*Photon.Realtime.Player[] list = PhotonNetwork.PlayerList;
            for (int i = 0; i < list.Length; i++) 
            {
                if (list[i].IsLocal)
                {
                    CreatePlayer();
                }
            }*/

            CreatePlayer();
        }

        private void CreatePlayer()
        {
            Player player = PhotonNetwork.Instantiate(_playerPrefab.name, Vector2.zero, Quaternion.identity).GetComponent<Player>();

            player.PhotonPlayer = PhotonNetwork.LocalPlayer;

            Singleton<GamePointsManager>.instance.CreateToken(player);
        }
    }
}