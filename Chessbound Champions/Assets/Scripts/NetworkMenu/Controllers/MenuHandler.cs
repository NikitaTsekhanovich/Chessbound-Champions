using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using SceneLoaderControllers;
using UnityEngine;

namespace NetworkMenu.Controllers
{
    public class MenuHandler : MonoBehaviourPunCallbacks
    {
        private const byte startGameEventCode = 1;

        public void StartGame()
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.RaiseEvent(startGameEventCode, null, new RaiseEventOptions { Receivers = ReceiverGroup.All }, SendOptions.SendReliable);
            }
            LoadingScreenController.Instance.ChangeScene("Game");
        }

        public void QuitGame()
        {
            Application.Quit();
        }

        public void BackToStartScene()
        {
            PhotonNetwork.Disconnect();
        }

        public override void OnDisconnected(Photon.Realtime.DisconnectCause cause)
        {
            LoadingScreenController.Instance.ChangeScene("StartScene");
        }
    }
}