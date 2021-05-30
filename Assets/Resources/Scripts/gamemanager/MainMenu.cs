using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using ExitGames.Client.Photon.Encryption;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviourPunCallbacks
{
    [Header("Login")]
    public GameObject inputNamePanel;

    [Header("Register")]
    public GameObject registerPanel;
    
    //public GameObject findOpponentPanel;
    //public GameObject waitingStatusPanel;
    
    [Header("Room Selector")]
    public GameObject roomPanel;
    
    [Header("Main Menu")]
    public GameObject mainMenuPanel;
    
    [Header("Create Room")]
    public GameObject createRoomPanel;
    public InputField roomNameInputField;
    public Button createButton;
    
    [Header("Room List")]
    public GameObject roomListPanel;
    public GameObject roomListEntryPrefab;
    public GameObject roomListContent;
    
    [Header("Inside Room")]
    public GameObject insideRoomPanel;
    public Button startGameButton;
    public GameObject playerListEntryPrefab;
    public GameObject listPanel;

    [Header("Join Random Room")]
    public GameObject joinRandomRoomPanel;

    [Header("Tutorial Panel")] 
    public GameObject tutorialPanel;
    

    private bool isConnecting = false;

    private const string GameVersion = "0.1"; // версии игроков должны быть одинаковы
    private const int MaxPlayersPerRoom = 2; // макс игроков

    private Dictionary<int, GameObject> _playerListEntries;
    private Dictionary<string, RoomInfo> _cachedRoomList;
    private Dictionary<string, GameObject> _roomListEntries;

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true; // штука для того чтобы ждать пока все загрузяться

        _cachedRoomList = new Dictionary<string, RoomInfo>();
        _roomListEntries = new Dictionary<string, GameObject>();
            
        //PlayerNameInput.text = "Player " + Random.Range(1000, 10000);
    }

    public void PlayTutorial()
    {
        PhotonNetwork.OfflineMode = true;
    }

    public override void OnConnectedToMaster()
    {
        if (PhotonNetwork.OfflineMode)
        {
            this.SetActivePanel(tutorialPanel.name);
        }
        else
        {
            this.SetActivePanel(roomPanel.name);
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {

        SetActivePanel(mainMenuPanel.name);
        
        Debug.Log($"Disconected due to: {cause}");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        string roomName = "Arena " + Random.Range(1, 100);

        RoomOptions options = new RoomOptions {MaxPlayers = MaxPlayersPerRoom};

        PhotonNetwork.CreateRoom(roomName, options, null);
    }

    public override void OnJoinedRoom()
    {
        if (!PhotonNetwork.OfflineMode)
        {
            SetActivePanel(insideRoomPanel.name);

            if (_playerListEntries == null)
            {
                _playerListEntries = new Dictionary<int, GameObject>();
            }

            foreach (Player p in PhotonNetwork.PlayerList)
            {
                GameObject entry = Instantiate(playerListEntryPrefab);
                entry.transform.SetParent(insideRoomPanel.transform);
                entry.transform.localScale = Vector3.one;
                entry.GetComponent<PlayerListEntry>().Initialize(p.ActorNumber, p.NickName);

                object isPlayerReady;
                if (p.CustomProperties.TryGetValue(MagickaGame.PLAYER_READY, out isPlayerReady))
                {
                    entry.GetComponent<PlayerListEntry>().SetPlayerReady((bool) isPlayerReady);
                }

                _playerListEntries.Add(p.ActorNumber, entry);
            }

            startGameButton.gameObject.SetActive(CheckPlayersReady());

            Hashtable props = new Hashtable
            
            {
                {MagickaGame.PLAYER_LOADED_LEVEL, false}
            };
            PhotonNetwork.LocalPlayer.SetCustomProperties(props);
        }
        
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        GameObject entry = Instantiate(playerListEntryPrefab);
        entry.transform.SetParent(insideRoomPanel.transform);
        entry.transform.localScale = Vector3.one;
        entry.GetComponent<PlayerListEntry>().Initialize(newPlayer.ActorNumber, newPlayer.NickName);

        _playerListEntries.Add(newPlayer.ActorNumber, entry);

        startGameButton.gameObject.SetActive(CheckPlayersReady());
    }
    
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Destroy(_playerListEntries[otherPlayer.ActorNumber].gameObject);
        _playerListEntries.Remove(otherPlayer.ActorNumber);

        startGameButton.gameObject.SetActive(CheckPlayersReady());
    }
    
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == newMasterClient.ActorNumber)
        {
            startGameButton.gameObject.SetActive(CheckPlayersReady());
        }
    }
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (_playerListEntries == null)
        {
            _playerListEntries = new Dictionary<int, GameObject>();
        }

        GameObject entry;
        if (_playerListEntries.TryGetValue(targetPlayer.ActorNumber, out entry))
        {
            object isPlayerReady;
            if (changedProps.TryGetValue(MagickaGame.PLAYER_READY, out isPlayerReady))
            {
                entry.GetComponent<PlayerListEntry>().SetPlayerReady((bool) isPlayerReady);
            }
        }

        startGameButton.gameObject.SetActive(CheckPlayersReady());
    }

    public void Exit()
    {
        Application.Quit();
    }
    
    public void SetActivePanel(string activePanel)
    {
        registerPanel.SetActive(activePanel.Equals(registerPanel.name));
        roomPanel.SetActive(activePanel.Equals(roomPanel.name));
        createRoomPanel.SetActive(activePanel.Equals(createRoomPanel.name));
        inputNamePanel.SetActive(activePanel.Equals(inputNamePanel.name));
        insideRoomPanel.SetActive(activePanel.Equals(insideRoomPanel.name));
        mainMenuPanel.SetActive(activePanel.Equals(mainMenuPanel.name));
        roomListPanel.SetActive(activePanel.Equals(roomListPanel.name));
        joinRandomRoomPanel.SetActive(activePanel.Equals(joinRandomRoomPanel.name));
        tutorialPanel.SetActive(activePanel.Equals(tutorialPanel.name));
    }
    
    public void OnStartGameButtonClicked()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;

        PhotonNetwork.LoadLevel("MainScene");
    }

    public void OnStartTutorialButtonClicked()
    {
        RoomOptions options = new RoomOptions {MaxPlayers = 1, PlayerTtl = 0};

        PhotonNetwork.CreateRoom("Arena " + Random.Range(1, 100), options, null);
        PhotonNetwork.LoadLevel("LevelDesign");
       // Application.LoadLevel("LevelDesign");
    }

    public void OnLogOutButtonClicked()
    {
        PhotonNetwork.Disconnect();
    }
    
    public void OnBackButtonClicked() // on click "Back" in Rooms
    {
        if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.LeaveLobby();
        }

        SetActivePanel(roomPanel.name);
    }
    public void OnRoomListButtonClicked() // on click "Room List"
    {
        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
        }

        SetActivePanel(roomListPanel.name);
    }
    
    public void OnJoinRandomRoomButtonClicked() // on click "Join Random Room"
    {
        SetActivePanel(joinRandomRoomPanel.name);

        PhotonNetwork.JoinRandomRoom();
    }
    
    public void OnCreateRoomButtonClicked()
    {
        string roomName = roomNameInputField.text;
        roomName = (roomName.Equals(string.Empty)) ? "Arena " + Random.Range(1, 100) : roomName;

        /*byte maxPlayers;
        byte.TryParse(MaxPlayersInputField.text, out maxPlayers);
        maxPlayers = (byte) Mathf.Clamp(maxPlayers, 2, 8);*/

        RoomOptions options = new RoomOptions {MaxPlayers = MaxPlayersPerRoom, PlayerTtl = 10000 };

        PhotonNetwork.CreateRoom(roomName, options, null);
    }
    
    public void OnLeaveGameButtonClicked()
    {
        PhotonNetwork.LeaveRoom();
    }

    
    //----- Register/Login
    public void OnRegisterButtonClicked()
    {
        SetActivePanel(inputNamePanel.name);
        SetActivePanel(registerPanel.name);
    }

    public void OnLoginButtonClicked()
    {
        
    }

    public void OnRegisterBackButtonClicked()
    {
        SetActivePanel(registerPanel.name);
        SetActivePanel(inputNamePanel.name);
    }
    
    //----- Register/Login

    private bool CheckPlayersReady()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return false;
        }

        foreach (Player p in PhotonNetwork.PlayerList)
        {
            object isPlayerReady;
            if (p.CustomProperties.TryGetValue(MagickaGame.PLAYER_READY, out isPlayerReady))
            {
                if (!(bool) isPlayerReady)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        return true;
    }
    
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        ClearRoomListView();

        UpdateCachedRoomList(roomList);
        UpdateRoomListView();
    }

    public override void OnLeftLobby()
    {
        _cachedRoomList.Clear();

        ClearRoomListView();
    }
    
    private void ClearRoomListView()
    {
        foreach (GameObject entry in _roomListEntries.Values)
        {
            Destroy(entry.gameObject);
        }

        _roomListEntries.Clear();
    }
    
    public override void OnLeftRoom()
    {
        SetActivePanel(roomPanel.name);

        foreach (GameObject entry in _playerListEntries.Values)
        {
            Destroy(entry.gameObject);
        }

        _playerListEntries.Clear();
        _playerListEntries = null;
    }
    
    public void LocalPlayerPropertiesUpdated()
    {
        startGameButton.gameObject.SetActive(CheckPlayersReady());
    }
    
    private void UpdateCachedRoomList(List<RoomInfo> roomList)
    {
        foreach (RoomInfo info in roomList)
        {
            // Remove room from cached room list if it got closed, became invisible or was marked as removed
            if (!info.IsOpen || !info.IsVisible || info.RemovedFromList)
            {
                if (_cachedRoomList.ContainsKey(info.Name))
                {
                    _cachedRoomList.Remove(info.Name);
                }

                continue;
            }

            // Update cached room info
            if (_cachedRoomList.ContainsKey(info.Name))
            {
                _cachedRoomList[info.Name] = info;
            }
            // Add new room info to cache
            else
            {
                _cachedRoomList.Add(info.Name, info);
            }
        }
    }

    private void UpdateRoomListView()
    {
        foreach (RoomInfo info in _cachedRoomList.Values)
        {
            GameObject entry = Instantiate(roomListEntryPrefab);
            entry.transform.SetParent(roomListContent.transform);
            entry.transform.localScale = Vector3.one;
            entry.GetComponent<RoomListEntry>().Initialize(info.Name, (byte)info.PlayerCount, info.MaxPlayers);

            _roomListEntries.Add(info.Name, entry);
        }
    }
}
