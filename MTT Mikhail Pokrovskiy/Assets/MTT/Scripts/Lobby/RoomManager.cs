using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MTT
{
    internal class RoomManager : MonoBehaviourPunCallbacks
    {
        [SerializeField] private InputField _roomName;
        public InputField RoomName => _roomName;

        [SerializeField] private UIModeButton _modeJoinButton;
        public UIModeButton ModeJoinButton => _modeJoinButton;

        [SerializeField] private UIModeButton _modeCreateButton;
        public UIModeButton ModeCreateButton => _modeCreateButton;

        [SerializeField] private UIModeMark _mark;
        public UIModeMark Mark => _mark;

        [SerializeField] private UIStartButton _startButton;
        public UIStartButton StartButton => _startButton;

        [SerializeField] private Text _roomCount;
        public Text RoomCount => _roomCount;

        private bool _createMode = true;
        private bool _avalible = false;
        TypedLobby _lobby;
        private List<RoomInfo> _rooms;


        private void Start()
        {
            _lobby = new TypedLobby("DefaultLobby", LobbyType.Default);
            LoadBalancingClient client = PhotonNetwork.NetworkingClient;
            client.OpJoinLobby(_lobby);

            _roomName.onSubmit.AddListener(OnRoomNameChanged);
            _modeCreateButton.Click += OnModeButtonClick;
            _modeJoinButton.Click += OnModeButtonClick;
            _startButton.Click += OnStartButtonClick;
        }

        private void OnRoomNameChanged(string name)
        {
            CheckRooms();
        }

        private void OnModeButtonClick(bool itsCreateButton)
        {
            _createMode = itsCreateButton;
            OnRoomNameChanged(_roomName.text);
            _mark.ChangePosition(itsCreateButton);
            
        }

        private void OnStartButtonClick()
        {
            if (!CheckRooms())
                return;
            if (_createMode)
            {
                RoomOptions roomOptions = new RoomOptions();
                roomOptions.MaxPlayers = 4;
                PhotonNetwork.CreateRoom(_roomName.text, roomOptions, _lobby);
            }
            else
            {
                PhotonNetwork.JoinRoom(_roomName.text);
            }
        }

        public override void OnJoinedLobby()
        {
            print(PhotonNetwork.CurrentLobby.Name);
        }

        private bool CheckRooms()
        {
            if (!string.IsNullOrEmpty(_roomName.text))
            {
                RoomInfo info = _rooms.Find(x => x.Name == _roomName.text);
                if (info != null)
                {
                    if (_createMode)
                    {
                        _startButton.ChangeText(UIStartButtonMode.Warming2);
                        return false;
                    }
                    else
                    {
                        _startButton.ChangeText(UIStartButtonMode.Join);
                        return true;
                    }
                }
                else
                {
                    if (_createMode)
                    {
                        _startButton.ChangeText(UIStartButtonMode.Create);
                        return true;
                    }
                    else
                    {
                        _startButton.ChangeText(UIStartButtonMode.Warming);
                        return false;
                    }
                }
            }
            else
            {
                _startButton.ChangeText(UIStartButtonMode.NoName);
                return false;
            }
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            _rooms = roomList;
            _roomCount.text = $"Сейчас открыто комнат: {roomList.Count}";

            _avalible = CheckRooms();
        }

        public override void OnConnectedToMaster()
        {
            print("&");
        }

        public override void OnJoinedRoom()
        {
            PhotonNetwork.LoadLevel("Game");
        }
    }
}