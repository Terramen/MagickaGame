using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    private Player_transform _playerTransform;
    
    public float speed;
    public Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        _playerTransform = FindObjectOfType<Player_transform>();
        Destroy(gameObject, 4f);
    }

    // Update is called once per frame
    void Update()
    {
       // transform.Translate(transform.right * speed * Time.deltaTime);
        rb.velocity = transform.up *  speed;
    }
    
    private void OnTriggerEnter2D(Collider2D collider)
    {
        Destroy(gameObject);
    }
}
