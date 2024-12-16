using NetworkMenu.Items;
using NetworkMenu.Services;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace NetworkMenu.Controllers
{
    public class RoomUpdateHandler : MonoBehaviourPunCallbacks
    {
        [SerializeField] private Transform _contentRoom;
        [SerializeField] private PlayerListItem _playerItem;
        [SerializeField] private GameObject _startGameButton;

        public override void OnEnable()
        {
            base.OnEnable();
            RoomService.OnReturnRoom += UpdateRoom;
        }
        
        public override void OnDisable()
        {
            base.OnDisable();
            RoomService.OnReturnRoom -= UpdateRoom;
        }

        public override void OnJoinedRoom()
        {
            UpdateRoom();
        }

        private void UpdateRoom()
        {
            ClearPlayerList();
            FillPlayerList();
            
            _startGameButton.SetActive(PhotonNetwork.IsMasterClient);
        }

        private void ClearPlayerList()
        {
            for (var i = 0; i < _contentRoom.childCount; i++)
            {
                Destroy(_contentRoom.GetChild(i).gameObject);
            }
        }

        private void FillPlayerList()
        {
            var players = PhotonNetwork.PlayerList;

            foreach (var player in players)
            {
                Instantiate(_playerItem, _contentRoom).SetInfo(player);
            }
        }
        
        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            Instantiate(_playerItem, _contentRoom).SetInfo(newPlayer);
        }
        
        public override void OnMasterClientSwitched(Player newMasterClient)
        {
            _startGameButton.SetActive(PhotonNetwork.IsMasterClient);
        }
    }
}