using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Shooting : MonoBehaviour
{
    public Rigidbody2D rb;
    [SerializeField] private GameObject skill;
    public Transform firePoint;
    public Transform circle;
    //[SerializeField] private float speed = 5f;
    private Elements _elements;

    void Awake()
    {
  //      player = GameObject.Find("/Player");
    }

    // Start is called before the first frame update
    void Start()
    {
        
        _elements = FindObjectOfType<Elements>();
    }

    // Update is called once per frame
    void Update()
    {
        if (CrossPlatformInputManager.GetButtonDown("ButtonAttack"))
        {
            skill = GameObject.FindGameObjectWithTag("Boulder");
            /*Debug.Log("Counter" + _elements.Counter + "Sprite" + _elements.SpriteRenderers[_elements.Counter - 1].sprite + "Sprite1" + _elements.SpriteArray[1]);
            Debug.Log("Check" + (_elements.SpriteRenderers[_elements.Counter - 1].sprite == _elements.SpriteArray[1]));*/
            if (_elements.Counter > 0 && _elements.SpriteRenderers[_elements.Counter - 1].sprite == _elements.SpriteArray[1])
            {
                Shoot();
            }
            foreach (var r in _elements.SpriteRenderers)
            {
                r.enabled = false;
                r.sprite = _elements.SpriteArray[3];
            }
            _elements.Counter = 0;
            // Debug.Log("Кнопка нажата");
        }
    }

    void Shoot()
    {
        Instantiate(skill, firePoint.position, firePoint.rotation);
    }

    /*private void OnTriggerEnter2D(Collider2D collider)
    {
        Instantiate(skill, firePoint.position, firePoint.rotation);
        Destroy(skill);
    }*/
}
