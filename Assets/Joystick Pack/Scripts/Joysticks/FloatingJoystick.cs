using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FloatingJoystick : Joystick
{
    //private Player_transform _playerTransform;
    
    protected override void Start()
    {
        /*_playerTransform = FindObjectOfType<Player_transform>();*/
        base.Start();
        background.gameObject.SetActive(false);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
        background.gameObject.SetActive(true);
        base.OnPointerDown(eventData);
        //_playerTransform.IsPressed = true;
        // Player_transform. = true;
    }
    
    public override void OnPointerUp(PointerEventData eventData)
    {
        
        background.gameObject.SetActive(false);
        base.OnPointerUp(eventData);
       // _playerTransform.IsPressed = false;
       // playerTransform.IsPressed = false;
    }
}