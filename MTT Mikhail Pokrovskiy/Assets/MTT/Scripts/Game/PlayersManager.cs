using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MTT
{
    internal class PlayersManager : MonoBehaviourPunCallbacks
    {
        [SerializeField] private Player _playerPrefab;
        [SerializeField] private List<GameObject> _players;
        [SerializeField] private Rect _playArea;

        private void Start()
        {
            SpawnPlayer();
        }

        private void SpawnPlayer()
        {
            GameObject player = PhotonNetwork.Instantiate(_playerPrefab.name,
                new Vector2(Random.Range(_playArea.xMin, _playArea.xMax),
                Random.Range(_playArea.yMin, _playArea.yMax)),
                Quaternion.identity);
            player.transform.parent = transform;

            _players.Add(player);
        }

#if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(_playArea.center, _playArea.size);
        }
#endif
    }
}