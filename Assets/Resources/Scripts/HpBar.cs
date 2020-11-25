using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Random = UnityEngine.Random;

public class HpBar : MonoBehaviourPun, IPunObservable
{
    [SerializeField] private PhotonView photonView;
    [SerializeField] private float hp;
    public Transform hpbar;
    public PlayerSpawner playerSpawner;

    [SerializeField] private StatusEffect statusEffect;

    public float Hp
    {
        get => hp;
        set => hp = value;
    }
    // Start is called before the first frame update
    void Start()
    {
        playerSpawner = FindObjectOfType<PlayerSpawner>();
        hp = 100;
    }

    // Update is called once per frame
    void Update()
    {
        if (hp <= 0)
        {
            StartCoroutine(Respawn());
        }
    }
    
    // TakeStatusDamage
    [PunRPC]
    public void TakeStatusDamageRPC(float damage, EffectType effectType)
    {
        if (photonView.IsMine)
        {
            statusEffect.ChangeEffectStatus(effectType, 4, damage);
        }
    }
    
    
    [PunRPC] 
    public void TakeDamageRPC(float damage)
    {
        if (photonView.IsMine)
        {
            if (damage > hp)
            {
                hp = 0;
            }
            else
            {
                hp -= damage; 
                Vector3 hpLocalScale = new Vector3(hpbar.localScale.x - damage/100, hpbar.localScale.y, hpbar.localScale.z);
                hpbar.localScale = hpLocalScale;
            }


        }

    }
    
    
    [PunRPC]
    IEnumerator Respawn()
    {
        hp = 100;
        hpbar.localScale = new Vector3(1,1,1);
        transform.position = playerSpawner.SpawnPoints[Random.Range(0, playerSpawner.SpawnPoints.Length)].position;
        // GetComponent<FloatingJoystick>().enabled = false;
        yield return new WaitForSeconds(1f);
       // GetComponent<FloatingJoystick>().enabled = true;
    }
    


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(hp);
            stream.SendNext(hpbar.localScale);
        }
        else
        {
            if (stream.ReceiveNext() is Vector3 v)
            {
                hpbar.localScale = v;
            }

            if (stream.ReceiveNext() is float f)
            {
                hp = f;
            }
            /*hpbar.localScale = (Vector3) stream.ReceiveNext();*/
            /*hp = (float) stream.ReceiveNext();*/
        }
    }
}
