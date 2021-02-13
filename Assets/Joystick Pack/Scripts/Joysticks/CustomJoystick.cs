using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomJoystick : Joystick
{
    public float MoveThreshold { get { return moveThreshold; } set { moveThreshold = Mathf.Abs(value); } }


    
    [SerializeField] private float moveThreshold = 1;
    [SerializeField] private JoystickType joystickType = JoystickType.Custom;
    [SerializeField] private GameObject spellTargetRaduis;
    [SerializeField] private RectTransform spellHandle;
    [SerializeField] private Image spellIcon;

    private Vector2 fixedPosition = Vector2.zero;

    public void SetMode(JoystickType joystickType)
    {
        this.joystickType = joystickType;
        if(joystickType == JoystickType.Custom)
        {
            background.anchoredPosition = fixedPosition;
            background.gameObject.SetActive(true);
        }
        else
            background.gameObject.SetActive(false);
    }

    protected override void Start()
    {
        base.Start();
        fixedPosition = background.anchoredPosition;
        SetMode(joystickType);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        spellTargetRaduis.SetActive(true);
        spellIcon.enabled = true;
        if(joystickType != JoystickType.Custom)
        {
            background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
            background.gameObject.SetActive(true);
        }
        OnDrag(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if(joystickType != JoystickType.Custom)
            background.gameObject.SetActive(false);
            spellHandle.anchoredPosition = Vector2.zero;
        base.OnPointerUp(eventData);
        spellTargetRaduis.SetActive(false);
        spellIcon.enabled = false;
    }

    protected override void HandleInput(float magnitude, Vector2 normalised, Vector2 radius, Camera cam)
    {
        if (joystickType == JoystickType.Custom)
        {
            spellHandle.anchoredPosition = normalised   * radius;
        }
        
        //Debug.Log(magnitude + "+" +  normalised);
        //Debug.Log(radius);
        base.HandleInput(magnitude, normalised, radius, cam);
    }
}
