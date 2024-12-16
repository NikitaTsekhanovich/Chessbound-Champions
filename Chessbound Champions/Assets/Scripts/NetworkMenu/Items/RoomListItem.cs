using NetworkMenu.Services;
using Photon.Realtime;
using TMPro;
using UnityEngine;

namespace NetworkMenu.Items
{
    public class RoomListItem : MonoBehaviour
    {
        [SerializeField] private TMP_Text _roomText;
        [SerializeField] private TMP_Text _countPlayersText;

        private RoomInfo _roomInfo;

        public RoomInfo RoomInfo => _roomInfo;

        public void SetInfo(RoomInfo roomInfo)
        {
            _roomInfo = roomInfo;
            _roomText.text = $"Room name: {_roomInfo.Name}";
            _countPlayersText.text = $"{_roomInfo.PlayerCount}/{_roomInfo.MaxPlayers}";
        }

        public void OpenRoom()
        {
            RoomService.Instance.JoinRoom(_roomInfo);
        }
    }
}