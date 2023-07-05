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

        private bool _active = true;

        private void Start()
        {
            UpdateCounter();
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
            if (Singleton<PlayerManager>.instance.IsGameReady(photonView.AmOwner))
            {
                //закрыть подключение
                _active = false;
            }
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(_active);
                if (!_active)
                {
                    gameObject.SetActive(false);
                    Singleton<PlayerManager>.instance.StartGame();
                }
            }
            else
            {
                _active = (bool)stream.ReceiveNext();
                if (!_active)
                {
                    gameObject.SetActive(false);
                    Singleton<PlayerManager>.instance.StartGame();
                }
            }
        }
    }
}