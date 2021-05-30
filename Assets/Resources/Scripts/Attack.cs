using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine;

public class Attack : MonoBehaviourPun, IPunObservable
{
    private Player_transform _playerTransform;
    
    [SerializeField] private float speed;
    [SerializeField] private float damage;
    [SerializeField] private float statusDamage;
    [SerializeField] private float statusTime;
    public Rigidbody2D rb;
    private PlayerSpawner playerSpawner;
    private HpBar _hpBar;

    [Header("SkillSprite")] [SerializeField]
    private GameObject childSprite;

    [SerializeField] private EffectType effectType;
    

    void Start()
    {
        playerSpawner = FindObjectOfType<PlayerSpawner>();
        if (!photonView.IsMine) // даем скиллу 11
        {
            gameObject.layer = 11;
            if (childSprite != null)
            {
                childSprite.layer = 11;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            if (speed > 0)
            {
                rb.velocity = transform.up *  speed;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
            
// 9 Enemy, 10 Player


            if (collider.gameObject.layer == 8 && collider.gameObject.layer != 13)
            {
                PhotonNetwork.Destroy(gameObject);
            }
            if (collider.gameObject.layer == 9 && collider.TryGetComponent(out HpBar hpBar)) // player hit enemy
            {
                if (photonView.IsMine)
                {
                    hpBar.photonView.RPC("TakeDamageRPC", RpcTarget.All, damage);
                    hpBar.photonView.RPC("TakeStatusDamageRPC", RpcTarget.All, statusDamage, effectType, statusTime);
                    PhotonNetwork.Destroy(gameObject);
                }
            }
            
    }

    private void OnDestroy()
    {
        if (photonView.IsMine)
        {
            if (playerSpawner.Shooting.ID == 1 || playerSpawner.Shooting.ID == 2 || playerSpawner.Shooting.ID == 3)
            {
                PhotonNetwork.Instantiate("Prefabs/Explosion", transform.position, transform.rotation);
               // Debug.Log("Сработал взрыв");
            }
            if (playerSpawner.Shooting.ID == 7 || playerSpawner.Shooting.ID == 8 || playerSpawner.Shooting.ID == 9)
            {
                PhotonNetwork.Instantiate("Prefabs/BoulderCrush", transform.position, transform.rotation);
               // Debug.Log("Сработали камни булыги");
            }
            if (playerSpawner.Shooting.ID == 4 || playerSpawner.Shooting.ID == 5 || playerSpawner.Shooting.ID == 6)
            {
                PhotonNetwork.Instantiate("Prefabs/IceLanceCrush", transform.position, transform.rotation);
                //Debug.Log("Сработ");
            }
        }
        //Debug.Log($"playerSpawner.Shooting.ID{playerSpawner.Shooting.ID}");

    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
          //  stream.SendNext(gameObject.GetComponent<HpBar>());
          
            stream.SendNext(rb.velocity);
           // stream.SendNext(playerSpawner.Shooting.ID);
        }
        else
        {
            /*if (stream.ReceiveNext() is GameObject obj)
            {
                gameObject.GetComponent<HpBar>() = obj;
            }*/
            /* if (stream.ReceiveNext() is int i)
            {
                playerSpawner.Shooting.ID = i;
            } */
            if (stream.ReceiveNext() is Vector2 v)
            {
                rb.velocity = v;
            }
        }

    }


}
