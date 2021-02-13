using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class TargetAttack : MonoBehaviourPun
{
    private Animator _animator;
    [SerializeField] private float damage;
    private bool isHitByLightning;
    // Start is called before the first frame update
    void Start()
    {
        if (!photonView.IsMine) // даем скиллу 11
        {
            gameObject.layer = 11;
        }
        _animator = GetComponent<Animator>();
        StartCoroutine(Delay());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    [PunRPC]
    IEnumerator Delay()
    {
        yield return new WaitForSeconds(_animator.GetCurrentAnimatorStateInfo(0).length);
        Destroy(transform.parent.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == 9 && collider.TryGetComponent(out HpBar hpBar))
        {
            if (photonView.IsMine)
            {
                if (!isHitByLightning)
                {
                    hpBar.photonView.RPC("TakeDamageRPC", RpcTarget.All, damage);
                    isHitByLightning = true;
                }
            }
        }
    }
}
