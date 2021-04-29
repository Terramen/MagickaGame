using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Random = UnityEngine.Random;

/*public class PlayerInfo
{
    public int actor;
    public short kills;
    public short deaths;

    public PlayerInfo(int actor, short kills, short deaths)
    {
        this.actor = actor;
        this.kills = kills;
        this.deaths = deaths;
    }
}*/
public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform[] spawnPoints;
    private Shooting _shooting;
    private ShootingOffline _shootingOffline;
    private Player3 _player3;
    private PlayerOfflineInputs _playerOfflineInputs;
    private HpBar _hpBar;
    [SerializeField] private Camera _camera;

    /*public List<PlayerInfo> playerinfo = new List<PlayerInfo>();
    public int myInd;

    public Text uiKills;
    public Text uiDeaths;*/

    /*[Header("Online mode?")]
    public bool isOnlineMode;*/

    public ShootingOffline ShootingOffline
    {
        get => _shootingOffline;
        set => _shootingOffline = value;
    }

    /*public bool IsOnlineMode
    {
        get => isOnlineMode;
        set => isOnlineMode = value;
    }*/

    public Shooting Shooting
    {
        get => _shooting;
        set => _shooting = value;
    }

    public Camera Camera
    {
        get => _camera;
        set => _camera = value;
    }

    public Player3 Player3
    {
        get => _player3;
        set => _player3 = value;
    }

    public PlayerOfflineInputs PlayerOfflineInputs
    {
        get => _playerOfflineInputs;
        set => _playerOfflineInputs = value;
    }

    public Transform[] SpawnPoints
    {
        get => spawnPoints;
        set => spawnPoints = value;
    }

    public HpBar HpBar
    {
        get => _hpBar;
        set => _hpBar = value;
    }
    /*
    private void Start()
    {
        if (isOnlineMode)
        {
            Spawn(); 
        }
        else
        {
            SpawnOffline();
        }
    }
    */
    
    private void Start()
    {
        Spawn();
    }

    
    public void Spawn()
    {
        Transform tSpawn = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject player = PhotonNetwork.Instantiate("Prefabs/Player", tSpawn.position, tSpawn.rotation); // playerPrefab.name
        _shooting = player.GetComponentInChildren<Shooting>();
        _camera = player.GetComponentInChildren<Camera>();
        _player3 = player.GetComponent<Player3>();
        _hpBar = player.GetComponent<HpBar>();
    }

    /*public void SpawnOffline()
    {
        _shootingOffline = playerPrefab.GetComponentInChildren<ShootingOffline>();
        _camera = playerPrefab.GetComponentInChildren<Camera>();
        _playerOfflineInputs = playerPrefab.GetComponent<PlayerOfflineInputs>();
        _hpBar = playerPrefab.GetComponent<HpBar>();
    }*/
}
