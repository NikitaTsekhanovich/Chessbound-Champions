using System;
using NetworkMenu.Validators;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

namespace NetworkMenu.Services
{
    public class RoomService : MonoBehaviourPunCallbacks
    {
        [SerializeField] private TMP_Text _roomTitle;
        [SerializeField] private GameObject _lobby;
        [SerializeField] private GameObject _room;
        [SerializeField] private GameObject _buttonsNavigate;
        [SerializeField] private GameObject _blockInput;
        [SerializeField] private TMP_InputField _createRoomInput;
        [SerializeField] private TMP_InputField _joinRoomInput;
        
        public static RoomService Instance;
        public static Action OnReturnRoom;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            _createRoomInput.characterLimit = 16;
            _joinRoomInput.characterLimit = 16;
        }
        
        public void CreateRoom()
        {
            var roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = 2;
          
            if (RoomCreateValidator.IsVoidName(_createRoomInput.text))
            {
                _room.SetActive(false);

                var placeholder = _createRoomInput.placeholder.GetComponent<TextMeshProUGUI>();
                _createRoomInput.text = "";
                placeholder.text = "Cannot be empty!";

                return;
            }

            _roomTitle.text = _createRoomInput.text;
            PhotonNetwork.CreateRoom(_createRoomInput.text, roomOptions);
        }
        
        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            Debug.LogWarning($"Error create room: {returnCode}");
            Debug.LogWarning($"Error create room: {message}");

            var placeholder = _createRoomInput.placeholder.GetComponent<TextMeshProUGUI>();
            _createRoomInput.text = "";
            placeholder.text = "Name taken!";
        }
        
        public override void OnCreatedRoom()
        {
            Debug.Log("Room created!");
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            Debug.LogWarning($"Error join room: {returnCode}");
            Debug.LogWarning($"Error join room: {message}");

            if (returnCode == 32765)
            {
                var placeholder = _joinRoomInput.placeholder.GetComponent<TextMeshProUGUI>();
                _joinRoomInput.text = "";
                placeholder.text = "Game full!";
                _blockInput.SetActive(false);
            }
            else
            {
                var placeholder = _joinRoomInput.placeholder.GetComponent<TextMeshProUGUI>();
                _joinRoomInput.text = "";
                placeholder.text = "Room not found!";
            }
        }
        
        public override void OnJoinedRoom()
        {
            _lobby.SetActive(false);
            _room.SetActive(true);
            _blockInput.SetActive(false);
        }
        
        public void JoinRoom(RoomInfo roomInfo)
        {
            _blockInput.SetActive(true);
            _roomTitle.text = roomInfo.Name;
            PhotonNetwork.JoinRoom(roomInfo.Name);
        }
        
        public void JoinRoomId()
        {
            if (RoomCreateValidator.IsVoidName(_joinRoomInput.text))
            {
                var placeholder = _joinRoomInput.placeholder.GetComponent<TextMeshProUGUI>();
                _joinRoomInput.text = "";
                placeholder.text = "Cannot be empty!";

                return;
            }

            _roomTitle.text = _joinRoomInput.text;
            PhotonNetwork.JoinRoom(_joinRoomInput.text);
        }
        
        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        public void ReturnRoom()
        {
            _buttonsNavigate.SetActive(false);
            _room.SetActive(true);
            OnReturnRoom?.Invoke();
        }

        // public override void OnPlayerLeftRoom(Player otherPlayer)
        // {
        //     Debug.Log("Игрок покинул комнату: " + otherPlayer.NickName);
        //     Debug.Log(otherPlayer.IsMasterClient);
        // }
    }
}

