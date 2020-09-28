using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_transform : MonoBehaviour
{

    public FloatingJoystick floatingJoystick;
    public float moveSpeed;
    public Rigidbody2D rb;
    public Vector2 movement;
    public Transform projectilePoint;
    public GameObject circleSprite;
    public GameObject circle;
    private float _аngle;

    private float _currentSpeed;

    public float CurrentSpeed => _currentSpeed;

    //public static bool isPressed;
    private bool _isPressed;

    public bool IsPressed
    {
        get { return _isPressed; }
        set { _isPressed = value; }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _currentSpeed = Mathf.Pow(rb.velocity.x, 2);
        movement = new Vector2(floatingJoystick.Horizontal, floatingJoystick.Vertical);
        moveCharacter(movement);
        // Vector3 movement = new Vector3(floatingJoystick.Horizontal, floatingJoystick.Vertical, 0f);
        // transform.position += movement * Time.deltaTime * moveSpeed;
        // _аngle = Mathf.Atan2(floatingJoystick.Vertical, floatingJoystick.Horizontal) * Mathf.Rad2Deg - 90f;
        //rb.rotation = angle;
         // circleRb.rotation = angle;
         if (_isPressed)
         {
             _аngle = Mathf.Atan2(floatingJoystick.Vertical, floatingJoystick.Horizontal) * Mathf.Rad2Deg - 90f;
         }
         circle.transform.rotation = Quaternion.Euler(0, 0, _аngle);
         circleSprite.transform.rotation = Quaternion.Euler(45f, 0, circle.transform.eulerAngles.z);
    }

    void moveCharacter(Vector2 direction)
    {
        rb.velocity = direction * moveSpeed;
      //  circleRb.velocity = direction * moveSpeed;
      // circle.transform.position = Vector2.MoveTowards(transform.position, transform.position, Time.deltaTime);
    }

}