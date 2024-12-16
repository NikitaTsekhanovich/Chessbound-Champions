using MainMenu;
using Photon.Pun;
using SceneLoaderControllers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NetworkControllers
{
    public class NetworkConnector : MonoBehaviourPunCallbacks
    {
        [SerializeField] private Button _singlePlayerButton;
        [SerializeField] private TMP_InputField _playerNameInput;
        [SerializeField] private TMP_Text _buttonText;
        private const int CharacterNameLimit = 16;
        private const string DefaultName = "Anonymous";

        private void Start()
        {
            // _playerNameInput.characterLimit = CharacterNameLimit;
            LoadName();
        }
        
        public override void OnEnable()
        {
            base.OnEnable();
            MenuController.OnStartMultiplayer += ConnectToNetwork;
        }
        
        public override void OnDisable()
        {
            base.OnDisable();
            MenuController.OnStartMultiplayer -= ConnectToNetwork;
        }
        
        public void LoadName()
        {
            // var playerSavedName = PlayerPrefs.GetString(PlayerDataKeys.NameKey);
            //
            // if (playerSavedName == "")
            // {
            //     _playerNameInput.text = DefaultName;
            //     PhotonNetwork.NickName = DefaultName;
            // }
            // else
            // {
            //     _playerNameInput.text = playerSavedName;
            //     PhotonNetwork.NickName = _playerNameInput.text;
            // }
        }
        
        public void SaveName()
        {
            var newName = _playerNameInput.text;
        
            if (newName != "")
            {
                // PlayerPrefs.SetString(PlayerDataKeys.NameKey, newName);
                PhotonNetwork.NickName = _playerNameInput.text;
            }
        }
        
        private void ConnectToNetwork()
        {
            if (IsInternetAvailable())
            {
                _singlePlayerButton.interactable = false;
                _buttonText.text = "Connecting...";
                PhotonNetwork.ConnectUsingSettings();
            }
            else
            {
                _buttonText.text = "Check your internet connection";
            }
        }
        
        public override void OnConnectedToMaster()
        {
            // PhotonNetwork.JoinLobby();
            PhotonNetwork.AutomaticallySyncScene = true;
            LoadingScreenController.Instance.ChangeScene("NetworkMenu");
        }
        
        private bool IsInternetAvailable()
        {
            return Application.internetReachability != NetworkReachability.NotReachable;
        }
    }
}
