using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class AttackParticle : MonoBehaviourPun
{
    
    [SerializeField] private float damage;
    [SerializeField] private float statusDamage;
    [SerializeField] private float statusTime;
    public PlayerSpawner playerSpawner;
  //  public Player3 player3;
   // private HpBar _hpBar;
    [SerializeField] private LayerMask layerEnemy;
    [SerializeField] private EffectType effectType;

    private bool isBurning;
    private bool isDoused;

    void Awake()
    {
        playerSpawner = FindObjectOfType<PlayerSpawner>();
    }

    void Start()
    {
        
        if (!photonView.IsMine) // даем скиллу 11
        {
            var collision = gameObject.GetComponent<ParticleSystem>().collision;
            collision.collidesWith = layerEnemy;
            gameObject.layer = 11;
        }

        
    }

    void OnEnable()
    {
        Debug.Log($"playerSpawner.Shooting.ParticleDamage:{playerSpawner.Shooting.ParticleDamage}");
        damage = playerSpawner.Shooting.ParticleDamage;
        statusTime = playerSpawner.Shooting.EffectTime;
    }

    private void OnDisable()
    {
        if (isBurning)
        {
            isBurning = false;
        }

        if (isDoused)
        {
            isDoused = false;
        }
    }


    private void OnParticleCollision(GameObject collider)
    {
        if (collider.gameObject.layer == 9 && collider.TryGetComponent(out HpBar hpBar))
        {
            if (photonView.IsMine)
            {
                hpBar.photonView.RPC("TakeDamageRPC", RpcTarget.All, damage);
                if (!isBurning)
                {
                    hpBar.photonView.RPC("TakeStatusDamageRPC", RpcTarget.All, statusDamage, effectType, statusTime);
                    isBurning = true;
                }

                if (!isDoused)
                {
                    hpBar.photonView.RPC("TakeStatusDamageRPC", RpcTarget.All, statusDamage, effectType, statusTime);
                    isDoused = true;
                }
                Debug.Log("Hit by particle");
            }
        }
    }
}
