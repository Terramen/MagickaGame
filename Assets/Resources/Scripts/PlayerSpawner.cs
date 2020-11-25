using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Random = UnityEngine.Random;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform[] spawnPoints;

    public Transform[] SpawnPoints
    {
        get => spawnPoints;
        set => spawnPoints = value;
    }
    private void Start()
    {

        /*if (PhotonNetwork.PlayerList.Length > 1)
        {
            PhotonNetwork.Instantiate("Prefabs/PlayerTwo", Vector3.zero, Quaternion.identity);
        }
        else
        {
            PhotonNetwork.Instantiate("Prefabs/Player", Vector3.zero, Quaternion.identity);
        }*/
        Spawn();
    }

    
    public void Spawn()
    {
        Transform tSpawn = spawnPoints[Random.Range(0, spawnPoints.Length)];
        PhotonNetwork.Instantiate("Prefabs/Player", tSpawn.position, tSpawn.rotation); // playerPrefab.name
    }
}
