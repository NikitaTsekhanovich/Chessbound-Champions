using System.Collections.Generic;
using NetworkMenu.Items;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace NetworkMenu.Controllers
{
    public class LobbyUpdateHandler : MonoBehaviourPunCallbacks
    {
        [SerializeField] private Transform _contentLobby;
        [SerializeField] private RoomListItem _roomListItem;
        private List<RoomListItem> _roomList = new ();

        private void Awake()
        {
            PhotonNetwork.JoinLobby();
        }
        
        public override void OnRoomListUpdate(List<RoomInfo> currentRoomList)
        {
            DeleteOldRoom(currentRoomList);
            AddUpdateRoom(currentRoomList);
        }

        private void AddUpdateRoom(List<RoomInfo> currentRoomList)
        {
            foreach (var currentRoom in currentRoomList)
            {
                if (currentRoom.IsOpen &&
                    currentRoom.IsVisible &&
                    currentRoom.PlayerCount > 0 &&
                    currentRoom.PlayerCount < currentRoom.MaxPlayers)
                {
                    var newRoom = Instantiate(_roomListItem, _contentLobby);
                    newRoom.SetInfo(currentRoom);
                    _roomList.Add(newRoom);
                }
            }
        }

        private void DeleteOldRoom(List<RoomInfo> currentRoomList)
        {
            for (var i = 0; i < _roomList.Count; i++)
            {
                if (currentRoomList.Count == 1 &&
                    _roomList[i].RoomInfo.Name == currentRoomList[0].Name)
                {
                    Destroy(_roomList[i].gameObject);
                    _roomList.Remove(_roomList[i]);
                }
            }
        }

        public override void OnConnectedToMaster()
        {
            foreach (var room in _roomList)
                Destroy(room.gameObject);

            _roomList.Clear();
        }
    }
}