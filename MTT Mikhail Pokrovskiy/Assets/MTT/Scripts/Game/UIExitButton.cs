using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MTT
{
    public class UIExitButton : MonoBehaviourPunCallbacks, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.LoadLevel("Loading");
        }
    }
}