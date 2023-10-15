using Photon.Pun;
using UnityEngine.SceneManagement;

namespace MTT
{
    internal class ServerConnect : MonoBehaviourPunCallbacks
    {
        private void Start()
        {
            PhotonNetwork.ConnectUsingSettings();
        }

        public override void OnConnectedToMaster()
        {
            SceneManager.LoadScene("Lobby");
        }

    }
}

