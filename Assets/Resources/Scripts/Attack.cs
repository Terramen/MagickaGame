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
    public Rigidbody2D rb;
    private PlayerSpawner playerSpawner;
    private HpBar _hpBar;

    [Header("SkillSprite")] [SerializeField]
    private GameObject childSprite;

    [SerializeField] private EffectType effectType;
    


    // Start is called before the first frame update
    void Start()
    {
        playerSpawner = FindObjectOfType<PlayerSpawner>();
        if (!photonView.IsMine) // даем скиллу 11
        {
            gameObject.layer = 11;
            childSprite.layer = 11;

        }
        // hpBar = g
        // Destroy(gameObject, 4f);
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            rb.velocity = transform.up *  speed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
            
// 9 Enemy, 10 Player

        /*if (collider.gameObject.layer == 10 && !photonView.IsMine) // enemy hit player
        {
           // collider.GetComponent<HpBar>().TakeDamage(damage);
           // photonView.RPC("DestroyRPC", RpcTarget.All); // Поменять потом на Pulling
        }*/

            if (collider.gameObject.layer == 8 && collider.gameObject.layer != 13)
            {
                PhotonNetwork.Destroy(gameObject);
            }
            if (collider.gameObject.layer == 9 && collider.TryGetComponent(out HpBar hpBar)) // player hit enemy
            {
                /*if (collider.GetComponent<StatusEffect>() != null && shooting.SkillName == "Skills/IceLance")    // Ледяное копье попадает по противнику и замораживает (было shooting.PrefabPool[2] == shooting.Skill)
                {
                    collider.GetComponent<StatusEffect>().ApplyFreeze(5);
                }
                if (collider.GetComponent<StatusEffect>() != null && shooting.SkillName == "Skills/Fireball")    // Фаербол попадает по противнику и поджигает 
                {
                    PhotonNetwork.Instantiate("Prefabs/Explosion", transform.position, transform.rotation);
                    if (!collider.GetComponent<StatusEffect>().IsBurning)
                    {
                        collider.GetComponent<StatusEffect>().ApplyBurn(4, 1);
                    }
                    
                    
                }*/
                if (photonView.IsMine)
                {
                    
                    hpBar.photonView.RPC("TakeDamageRPC", RpcTarget.All, damage);
                    hpBar.photonView.RPC("TakeStatusDamageRPC", RpcTarget.All, statusDamage, effectType);
                    PhotonNetwork.Destroy(gameObject);
                }
            }
            
               /*else if (photonView.IsMine)
                {
                    Debug.Log("!!!!!!!!!!!!!!!!!!!!! " + collider.gameObject.name);
                   // photonView.RPC("DestroyRPC", RpcTarget.All);
                }*/

                /*if (collider.gameObject.tag == "Enemy")
                {
                    Destroy(gameObject);
                }*/
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
