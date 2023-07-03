using Photon.Pun;
using Unity.VisualScripting;

namespace MTT
{
    internal enum ControlType
    {
        Left,
        Right,
        Up,
        Down,
        Shoot
    }

    [Singleton]
    internal class Controls : MonoBehaviourPunCallbacks, ISingleton
    {
        public delegate void ControlHendler(Photon.Realtime.Player player, ControlType control);
        public event ControlHendler Control;

        public void TakeAction(ControlType control)
        {
            Control?.Invoke(PhotonNetwork.LocalPlayer, control);
        }
    }
}