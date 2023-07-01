using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace MTT
{
    [RequireComponent(typeof(PhotonView))]
    internal class Player : MonoBehaviour
    {
        [SerializeField] private PhotonView _view;

        private void Update()
        {
        }
    }
}


