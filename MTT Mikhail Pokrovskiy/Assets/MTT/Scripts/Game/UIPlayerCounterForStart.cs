using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MTT
{
    public class UIPlayerCounterForStart : MonoBehaviourPunCallbacks, IPointerClickHandler, IPunObservable
    {
        [SerializeField] private Text _counter;

        private bool _playersReady = false;
        private bool _playerCreated = false;
        private bool _tokenCreated = false;
        private bool _gameReady = false;

        private float _timer = 0;

        private void Start()
        {
            UpdateCounter();
        }

        private void FixedUpdate()
        {
            if (_gameReady)
            {
                _timer += Time.deltaTime;
                if (_timer > 2)
                {
                    Singleton<GamePointsManager>.instance.StartGame();
                    gameObject.SetActive(false);
                }
            }
        }

        public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
        {
            UpdateCounter();
        }

        private void UpdateCounter()
        {
            _counter.text = $"<{PhotonNetwork.PlayerList.Length}/4>";
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (Singleton<PlayerManager>.instance.IsPlayersReady(photonView.AmOwner))
            {
                //закрыть подключение
                _playersReady = true;
            }
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(_playersReady);
                stream.SendNext(_tokenCreated);
                print($"Отправлено: {_playersReady};{_tokenCreated}");
                if (_playersReady && !_playerCreated)
                {
                    //поменять экран, но пока
                    _counter.text = "Загрузка";
                    Singleton<PlayerManager>.instance.CreatePlayer();
                    _playerCreated = true;
                }

                if (_playerCreated)
                {
                    if (!_tokenCreated)
                        _tokenCreated = Singleton<GamePointsManager>.instance.CheckTokens();
                    if (_tokenCreated)
                        _gameReady = true;
                }
            }
            else
            {
                _playersReady = (bool)stream.ReceiveNext();
                bool otherTokensCreated = (bool)stream.ReceiveNext();
                print($"Полчучено: {_playersReady};{otherTokensCreated}");

                if (_playersReady && !_playerCreated)
                {
                    //поменять экран, но пока
                    _counter.text = "Загрузка";
                    Singleton<PlayerManager>.instance.CreatePlayer();
                    _playerCreated = true;
                }

                if (_playerCreated)
                {
                    if (!_tokenCreated)
                        _tokenCreated = Singleton<GamePointsManager>.instance.CheckTokens();
                    if (_tokenCreated && otherTokensCreated)
                        _gameReady = true;
                    else if (!otherTokensCreated)
                    {
                        //поменять экран, но пока
                        _counter.text = "Ждем";
                        _gameReady = false;
                        _timer = 0;
                    }
                }
            }
        }
    }
}