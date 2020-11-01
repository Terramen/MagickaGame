using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    private Player_transform _playerTransform;
    
    public float speed;
    [SerializeField] private int damage;
    public Rigidbody2D rb;
    public Shooting shooting;
    private HpBar _hpBar;
    
    
    // Start is called before the first frame update
    void Start()
    {
       // hpBar = g
        shooting = FindObjectOfType<Shooting>();
        _playerTransform = FindObjectOfType<Player_transform>();
        Destroy(gameObject, 4f);
    }

    // Update is called once per frame
    void Update()
    {
        if (shooting.PrefabPool[0] || shooting.PrefabPool[1])
        {
            rb.velocity = transform.up *  speed;
        }
        // transform.Translate(transform.right * speed * Time.deltaTime);
    }
    
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.GetComponent<StatusEffect>() != null && shooting.PrefabPool[2] == shooting.Skill)    // Ледяное копье попадает по противнику и замораживает
        {
            for (int i = 0; i < shooting.PrefabPool.Length; i++)
            {
                Debug.Log(shooting.PrefabPool[i]);
            }
            collider.GetComponent<StatusEffect>().ApplyFreeze();
        }
        if (collider.GetComponent<StatusEffect>() != null && shooting.PrefabPool[1] == shooting.Skill)    // Фаербол попадает по противнику и поджигает
        {
            collider.GetComponent<StatusEffect>().ApplyBurn(4);
        }
        if (collider.GetComponent<HpBar>() != null && collider.gameObject.tag != "Player")
        {
            collider.GetComponent<HpBar>().TakeDamage(damage);
            /*Debug.Log(_hpBar.Hp);*/
            //Destroy(gameObject);
        }
        if (collider.gameObject.tag == "Enemy")
        {
            Destroy(gameObject);
        }
    }
    
}
