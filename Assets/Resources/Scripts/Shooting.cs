using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Unity.Mathematics;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Shooting : MonoBehaviourPun, IPunObservable
{
    public Rigidbody2D rb;

    // [SerializeField] private GameObject skill;     // для одичночки
    // [SerializeField] private UnityEngine.Object skill;
    public Transform firePoint;
    public Transform circle;
    //[SerializeField] private float speed = 5f;
    [SerializeField] private Elements elements;
    private GameObject[] _prefabPool;
    [SerializeField] private int raycastCount;
    private float raycastArc = 0;


    /*public GameObject Skill
    {
        get => skill;
        set => skill = value;
    }*/

    private String skillName;

    public string SkillName
    {
        get => skillName;
        set => skillName = value;
    }

    public GameObject[] PrefabPool => _prefabPool;
    
    [SerializeField] private GameObject waterflow;

    private bool _waterflowIsActive = false;


    // [SerializeField] private PhotonView _photonView;
   // private HpBar _hpBar;
  // [SerializeField] private PhotonView photonView;
   [SerializeField] private ParticleSystem waterflow2;


   // Start is called before the first frame update
   
   void Start()
   {
       waterflow2 = waterflow.GetComponent<ParticleSystem>();
        /*if (!photonView.IsMine)
        {
            gameObject.SetActive(false);
        }*/
        PrefabCreation();
        // skill = Resources.Load("Assets/Magicka/Prefab/Boulder");
        elements = FindObjectOfType<Elements>();
    }
    // Update is called once per frame
    void Update()
    {
        if (_waterflowIsActive)
        {
            waterflow.SetActive(true);
        }
        else waterflow.SetActive(false);
        if (photonView.IsMine)
        {
            if (CrossPlatformInputManager.GetButtonDown("ButtonAttack"))
                    {
                        if (elements.SpriteRenderers[0].sprite == elements.SpriteArray[1]) // Fireball
                        {
                            skillName = "Skills/Fireball";
                            // skill = _prefabPool[1];
                            PhotonNetwork.Instantiate(skillName, firePoint.position, firePoint.rotation);
                        }
                        if (elements.SpriteRenderers[0].sprite == elements.SpriteArray[2])
                        {
                            skillName = "Skills/Boulder";
                            //skill = _prefabPool[0];
                            PhotonNetwork.Instantiate(skillName, firePoint.position, firePoint.rotation);
                        }

                        if (elements.SpriteRenderers[0].sprite == elements.SpriteArray[0])
                        {
                            skillName = "Skills/Waterflow2";
                            StartCoroutine(ShootLong(4f));
                        }


                        if (elements.SpriteRenderers[0].sprite == elements.SpriteArray[6]) // Ice Lance
                        {
                            skillName = "Skills/IceLance";
                           // skill = _prefabPool[2];
                           PhotonNetwork.Instantiate(skillName, firePoint.position, firePoint.rotation);
                        }
                        foreach (var r in elements.SpriteRenderers)
                        {
                            r.enabled = false;
                            r.sprite = elements.SpriteArray[3];
                        }
                        elements.Counter = 0;
                    }
        }
    }

    private IEnumerator ShootLong(float time)
    {
        _waterflowIsActive = true;
        while (time > 0)
        {
            time -= Time.deltaTime; // Time.deltaTime нужно поменять
            yield return null;
        }
        _waterflowIsActive = false;
      //  waterflow2.shape.arc = 10 - 
    }

    /*private void WaterflowRaycast2D()
    {
       // raycastArc = waterflow2.shape.arc;

       while (raycastArc <= waterflow2.shape.arc)
        {
            --raycastCount;
            if (raycastCount > 0)
            {
                raycastArc = raycastArc + waterflow2.shape.arc / raycastCount;
            }
            RaycastHit2D hit = Physics2D.Raycast(firePoint.position, new Vector3(0,0, circle.transform.rotation.z - raycastArc), waterflow2.shape.radius);
            Debug.DrawRay(firePoint.position, new Vector3(0, 0 , circle.transform.rotation.z - raycastArc), Color.green);

            //
            HpBar hpBar = hit.collider.GetComponent<HpBar>();
            if (hpBar != null)
            {
                hpBar.TakeDamage(0.1f);
            }
        }

       raycastArc = 0;
       /*for (int i = 0; i < raycastCount; i++)
       {
           circle.transform.up.
           var raycastDir = circle.transform.rotation.z - waterflow2.shape.arc;
           RaycastHit2D hit = Physics2D.Raycast(firePoint.position, circle.transform.rotation.z - waterflow2.shape.arc, waterflow2.shape.radius, wallLayer);
           raycastArc = raycastArc - (raycastArc / raycastCount - 1);
       }#1#
    }*/

    void PrefabCreation()
    {
        _prefabPool = Resources.LoadAll<GameObject>("Skills");
       // Debug.Log(_prefabPool.Length);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(_waterflowIsActive);
        }
        else
        {
            if (stream.ReceiveNext() is bool b)
            {
                _waterflowIsActive = b;
            }
        }
    }
}
