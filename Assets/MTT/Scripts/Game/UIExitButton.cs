using Photon.Pun;
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