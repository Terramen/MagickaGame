using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityStandardAssets.CrossPlatformInput;

public class Shooting : MonoBehaviourPun, IPunObservable
{
    public Rigidbody2D rb;
    public Transform firePoint;
    public Transform circle;
    [SerializeField] private Elements2 elements;
    private GameObject[] _prefabPool;
    [SerializeField] private int raycastCount;
    private float raycastArc = 0;
    private float particleDamage = 0.05f;
    private float effectTime;
    private float multiplier = 1f;

    public float Multiplier
    {
        get => multiplier;
        set => multiplier = value;
    }

    public float ParticleDamage
    {
        get => particleDamage;
        set => particleDamage = value;
    }

    public float EffectTime
    {
        get => effectTime;
        set => effectTime = value;
    }
    

    private String skillName;

    public string SkillName
    {
        get => skillName;
        set => skillName = value;
    }

    public GameObject[] PrefabPool => _prefabPool;
    
    [SerializeField] private GameObject waterflow;
    [SerializeField] private GameObject lava;

    private bool _waterflowIsActive;
    private bool _lavaIsActive;

    public bool WaterflowIsActive
    {
        get => _waterflowIsActive;
        set => _waterflowIsActive = value;
    }

    public bool LavaIsActive
    {
        get => _lavaIsActive;
        set => _lavaIsActive = value;
    }

    // [SerializeField] private PhotonView _photonView;
   // private HpBar _hpBar;
   [SerializeField] private PhotonView photonView;
   [SerializeField] private ParticleSystem waterflow2;

   private Player3 _player3;

   private JoystickScr _joystickScr;
   
   private int currentSpellID;

   private Coroutine _coroutine;

   private int id = 0;

   public int ID
   {
       get => id;
       set => id = value;
   }

   public Coroutine Coroutine
   {
       get => _coroutine;
       set => _coroutine = value;
   }
   
   public IEnumerator ShootWaterflow(float time)
   {
       _waterflowIsActive = true;
       while (time > 0)
       {
           time -= Time.deltaTime;
           yield return new WaitForFixedUpdate();
       }
       _waterflowIsActive = false;
   }
   
   
   public IEnumerator ShootLava(float time)
   {
       _lavaIsActive = true;
       while (time > 0)
       {
           time -= Time.deltaTime;
           yield return new WaitForFixedUpdate();
       }
       _lavaIsActive = false;
   }

   public void Update()
   {
       if (_lavaIsActive)
       {
           lava.SetActive(true);
       }
       else
       {
           lava.SetActive(false);
       }
       if (_waterflowIsActive)
       {
           waterflow.SetActive(true);
       }
       else
       {
           waterflow.SetActive(false);
       }
   }


   public void StartAttack(Spell spell)
    {
        switch (spell.ID)
                        {
                            case 1:
                                spell.PutOnCooldown();
                                id = spell.ID;
                                effectTime = 0.75f;
                                PhotonNetwork.Instantiate(spell.spellName, firePoint.position, circle.rotation); //Fireball
                                break;
                            case 2:
                                spell.PutOnCooldown();
                                id = spell.ID;
                                effectTime = 0.75f;
                                PhotonNetwork.Instantiate(spell.spellName, firePoint.position, circle.rotation);
                                break;
                            case 3:
                                spell.PutOnCooldown();
                                id = spell.ID;
                                effectTime = 0.75f;
                                PhotonNetwork.Instantiate(spell.spellName, firePoint.position, circle.rotation);
                                break;
                            
                            case 7:
                                spell.PutOnCooldown();
                                id = spell.ID;
                                PhotonNetwork.Instantiate(spell.spellName, firePoint.position, circle.rotation);   //Boulder
                                break;
                            case 8:
                                spell.PutOnCooldown();
                                id = spell.ID;
                                PhotonNetwork.Instantiate(spell.spellName, firePoint.position, circle.rotation);
                                break;
                            case 9:
                                spell.PutOnCooldown();
                                id = spell.ID;
                                PhotonNetwork.Instantiate(spell.spellName, firePoint.position, circle.rotation);
                                break;
                            
                            case 4:
                                spell.PutOnCooldown();
                                id = spell.ID;
                                effectTime = 2f;
                                PhotonNetwork.Instantiate(spell.spellName, firePoint.position, circle.rotation);   //IceLance
                                break;
                            case 5:
                                spell.PutOnCooldown();
                                id = spell.ID;
                                effectTime = 2f;
                                PhotonNetwork.Instantiate(spell.spellName, firePoint.position, circle.rotation);
                                break;
                            case 6:
                                spell.PutOnCooldown();
                                id = spell.ID;
                                effectTime = 2f;
                                PhotonNetwork.Instantiate(spell.spellName, firePoint.position, circle.rotation);
                                break;
                            
                            case 10:
                                particleDamage = 0.05f;
                                effectTime = 3f;
                                _coroutine = StartCoroutine(ShootWaterflow(4f));    //Waterflow
                                break;
                            case 11:
                                particleDamage = 0.07f;
                                effectTime = 5f;
                                _coroutine = StartCoroutine(ShootWaterflow(4f)); 
                                break;
                            case 12: 
                                particleDamage = 0.09f;
                                effectTime = 7f;
                                _coroutine = StartCoroutine(ShootWaterflow(4f)); 
                                break;
                            
                            case 16:
                                particleDamage = 0.07f;
                                effectTime = 0.75f;
                                _coroutine = StartCoroutine(ShootLava(4f));    //Lava
                                break;
                            case 17:
                                particleDamage = 0.09f;
                                effectTime = 0.75f;
                                _coroutine = StartCoroutine(ShootLava(4f)); 
                                break;
                            case 18: 
                                particleDamage = 0.11f;
                                effectTime = 0.75f;
                                _coroutine = StartCoroutine(ShootLava(4f)); 
                                break;
                        }
    }

    public void StartTargetAttack(Spell spell, Vector3 point)
    {
        switch (spell.ID)
        {
            
            case 13: 
                spell.PutOnCooldown();
                PhotonNetwork.Instantiate(spell.spellName, point, Quaternion.identity);
                break;
            case 14:
                spell.PutOnCooldown();
                PhotonNetwork.Instantiate(spell.spellName, point, Quaternion.identity);
                break;
            case 15: 
                spell.PutOnCooldown();
                PhotonNetwork.Instantiate(spell.spellName, point, Quaternion.identity);
                break;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(_waterflowIsActive);
            stream.SendNext(_lavaIsActive);
        }
        else
        {
            /*if (stream.ReceiveNext() is GameObject obj)
            {
                gameObject.GetComponent<HpBar>() = obj;
            }*/
            if (stream.ReceiveNext() is bool b)
            {
                _waterflowIsActive = b;
            }
            if (stream.ReceiveNext() is bool b1)
            {
                _lavaIsActive = b1;
            }
        }
    }
}
