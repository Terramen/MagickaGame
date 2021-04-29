using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gamemanager : MonoBehaviour
{
    public SpellBase SpellBase;
    public ElementController elementController;

    
    [Header("Main Camera")]
    public GameObject mainCamera;

    public GameObject MainCamera
    {
        get => mainCamera;
        set => mainCamera = value;
    }

    private void Awake()
    {
        mainCamera.SetActive(false);
    }
}
