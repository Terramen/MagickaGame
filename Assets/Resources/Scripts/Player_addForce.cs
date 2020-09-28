using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    public FloatingJoystick floatingJoystick;
    public Rigidbody2D rb;

    public void Update()
    {
        Vector2 direction = Vector2.right * floatingJoystick.Horizontal + Vector2.up * floatingJoystick.Vertical;
        rb.AddForce(direction * speed * Time.fixedDeltaTime, ForceMode2D.Impulse);
    }
}
