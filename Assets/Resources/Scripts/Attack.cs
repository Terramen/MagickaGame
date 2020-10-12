using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    private Player_transform _playerTransform;
    
    public float speed;
    public int damage;
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
        if (collider.gameObject.tag == "Enemy")
        {
            _hpBar = collider.gameObject.GetComponent<HpBar>();
            _hpBar.TakeDamage(damage);
                Debug.Log(_hpBar.Hp);
            Destroy(gameObject);
        }
        if (collider.gameObject.tag != "Player")
        {
            Destroy(gameObject);
        }
    }
}
