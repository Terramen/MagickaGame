using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class AttackParticle : MonoBehaviourPun
{
    
    [SerializeField] private float damage;
    public Shooting shooting;
    private HpBar _hpBar;
    [SerializeField] private LayerMask layerEnemy;
    [SerializeField] private EffectType effectType;

    // Start is called before the first frame update
    void Start()
    {
        
        if (!photonView.IsMine) // даем скиллу 11
        {
            var collision = gameObject.GetComponent<ParticleSystem>().collision;
            collision.collidesWith = layerEnemy;
            gameObject.layer = 11;
        }
        shooting = FindObjectOfType<Shooting>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnParticleCollision(GameObject collider)
    {
        if (collider.gameObject.layer == 9 && collider.TryGetComponent(out HpBar hpBar))
        {
            if (shooting.SkillName == "Skills/Waterflow2")
            {
                hpBar.photonView.RPC("TakeDamageRPC", RpcTarget.All, damage);
               // hpBar.photonView.RPC("TakeStatusDamageRPC", RpcTarget.All, damage, effectType);
                Debug.Log("Hit by particle");
            }
        }
    }
}
