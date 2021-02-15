using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using ExitGames.Client.Photon.Encryption;

public class MainMenu : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject findOpponentPanel;
    [SerializeField] private GameObject waitingStatusPanel;
    [SerializeField] private Text waitingStatusText;

    [Header("Input Name")]
    public GameObject inputNamePanel;
    
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

    [Header("Join Random Room")]
    public GameObject joinRandomRoomPanel;
    

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

    /*public void FindOpponent()
    {
        isConnecting = true;
        
        findOpponentPanel.SetActive(false);
        waitingStatusPanel.SetActive(true);

        waitingStatusText.text = "Searching...";

        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            PhotonNetwork.GameVersion = GameVersion;
            PhotonNetwork.ConnectUsingSettings();
        }

        
    }*/

    public override void OnConnectedToMaster()
    {
        this.SetActivePanel(mainMenuPanel.name);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        waitingStatusPanel.SetActive(false);
        findOpponentPanel.SetActive(true);
        
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

        ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable
            
        {
            {MagickaGame.PLAYER_LOADED_LEVEL, false}
        };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
    }

    /*public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == MaxPlayersPerRoom)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;

            waitingStatusText.text = "Opponent found";
            Debug.Log("Match is ready to begin");
            
            PhotonNetwork.LoadLevel("MainScene");
        }
    }*/
    
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

    public void Exit()
    {
        Application.Quit();
    }
    
    public void SetActivePanel(string activePanel)
    {
        roomPanel.SetActive(activePanel.Equals(roomPanel.name));
        createRoomPanel.SetActive(activePanel.Equals(createRoomPanel.name));
        inputNamePanel.SetActive(activePanel.Equals(inputNamePanel.name));
        insideRoomPanel.SetActive(activePanel.Equals(insideRoomPanel.name));
        mainMenuPanel.SetActive(activePanel.Equals(mainMenuPanel.name));
        roomListPanel.SetActive(activePanel.Equals(roomListPanel.name));
        joinRandomRoomPanel.SetActive(activePanel.Equals(joinRandomRoomPanel.name));
    }
    
    public void OnStartGameButtonClicked()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;

        PhotonNetwork.LoadLevel("MainScene");
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
