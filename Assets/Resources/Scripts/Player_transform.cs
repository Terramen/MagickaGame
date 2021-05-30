using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.EventSystems;

public class Player_transform : MonoBehaviourPun, IPunObservable
{


public Animator animator;
   // public FloatingJoystick floatingJoystick;
    public Joystick joystick;
    public JoystickScr customJoystick;
    public float moveSpeed;
    public Rigidbody2D rb;
    public Vector2 movement;
    public Transform projectilePoint;
    public GameObject circleSprite;
    public GameObject circle;
   // public GameObject hpbar;
    private float _аngle;
    

    private float _currentSpeed;

    //private PhotonView photonView;

    [SerializeField] private SpriteRenderer spriteRenderer;
    
    public ShopItemBase shopItemBase;
    private int _skinID = 0;

    public float CurrentSpeed => _currentSpeed;

    // Start is called before the first frame update
    void Start()
    {
        //photonView = GetComponent<PhotonView>();
        joystick = FindObjectOfType<FloatingJoystick>();
        customJoystick = FindObjectOfType<JoystickScr>();
        //_animController = GetComponentInChildren<Animator>();
        if (photonView.IsMine)
        {
            photonView.RPC("ChangeSkin", RpcTarget.All);
        }
        if (!photonView.IsMine)
        {
            gameObject.layer = 9;
            circleSprite.SetActive(false);
            Destroy(GetComponentInChildren<Camera>().gameObject);
            
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (photonView.IsMine)
        {
               movement = new Vector2(joystick.Horizontal, joystick.Vertical);
               moveCharacter(movement);
               
               
               /*if (joystick.IsPressed)
               {
                   _аngle = Mathf.Atan2(joystick.Vertical, joystick.Horizontal) * Mathf.Rad2Deg - 90f;
               }*/
               _аngle = Mathf.Atan2(customJoystick.Vertical(), customJoystick.Horizontal()) * Mathf.Rad2Deg - 90f;

               circle.transform.rotation = Quaternion.Euler(0, 0, _аngle);
               circleSprite.transform.rotation = Quaternion.Euler(45f, 0, circle.transform.eulerAngles.z);
               animator.SetFloat("moveX", Math.Abs(rb.velocity.x));
               animator.SetFloat("moveY", rb.velocity.y);

               if (rb.velocity.x < 0)
               {
                   spriteRenderer.flipX = true;
               }
               else spriteRenderer.flipX = false;
        }
        
        // Vector3 movement = new Vector3(floatingJoystick.Horizontal, floatingJoystick.Vertical, 0f);
        // transform.position += movement * Time.deltaTime * moveSpeed;
        // _аngle = Mathf.Atan2(floatingJoystick.Vertical, floatingJoystick.Horizontal) * Mathf.Rad2Deg - 90f;
        //rb.rotation = angle;
         // circleRb.rotation = angle;
    }

    void moveCharacter(Vector2 direction)
    {
        rb.velocity = direction * moveSpeed;
      //  circleRb.velocity = direction * moveSpeed;
      // circle.transform.position = Vector2.MoveTowards(transform.position, transform.position, Time.deltaTime);
    }

    [PunRPC]
    public void ChangeSkin()
    {
        _skinID = shopItemBase.shopItems[ShopItemManager.instance.CurrentItemID].ID;
        animator.runtimeAnimatorController = shopItemBase.shopItems[_skinID].controller;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(spriteRenderer.flipX);
        }
        else
        {
            if (stream.ReceiveNext() is bool b) {
                spriteRenderer.flipX = b;
            }
            
        }
    }
}