using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

namespace NetworkMenu.Items
{
    public class PlayerListItem : MonoBehaviourPunCallbacks
    {
        [SerializeField] private TMP_Text _playerNameText;

        private Player _player;

        public void SetInfo(Player player)
        {
            _player = player;
            _playerNameText.text = $"Name: {_player.NickName}";
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            if (_player == otherPlayer)
            {
                Destroy(gameObject);
            }
        }
        
        public override void OnLeftRoom()
        {
            Destroy(gameObject);
        }
    }
}