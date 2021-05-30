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
    private Player3 _player3;
    private HpBar _hpBar;
    private Animator _animController;
    [SerializeField] private Camera _camera;

    public ShopItemBase shopItemBase;
    
    

    public Animator AnimController
    {
        get => _animController;
        set => _animController = value;
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
        _animController = player.GetComponentInChildren<Animator>();
        _shooting = player.GetComponentInChildren<Shooting>();
        _camera = player.GetComponentInChildren<Camera>();
        _player3 = player.GetComponent<Player3>();
        _hpBar = player.GetComponent<HpBar>();
       // _animController.runtimeAnimatorController = shopItemBase.shopItems[ShopItemManager.instance.CurrentItemID].controller;
    }
    
    

    /*public void SpawnOffline()
    {
        _shootingOffline = playerPrefab.GetComponentInChildren<ShootingOffline>();
        _camera = playerPrefab.GetComponentInChildren<Camera>();
        _playerOfflineInputs = playerPrefab.GetComponent<PlayerOfflineInputs>();
        _hpBar = playerPrefab.GetComponent<HpBar>();
    }*/
}
