using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private Animator _animator;
    // Start is called before the first frame update
    void Start()
    {
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
}
