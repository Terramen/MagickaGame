using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player3 : MonoBehaviourPun
{
    [SerializeField] private Gamemanager _gamemanager;
    [SerializeField] private List<Elements> spellElements = new List<Elements>();

    //private Spell spell;
   // [SerializeField] SpellUIController spellUIController;
    public ElementController elementController;

    [SerializeField] private JoystickScr joystickScr;
    
    

    /*public Spell Spell
    {
        get => spell;
        set => spell = value;
    }*/

    private void Start()
    {
        joystickScr = FindObjectOfType<JoystickScr>();
        _gamemanager = FindObjectOfType<Gamemanager>();
        elementController = _gamemanager.elementController;
    }
    
    void Update()
    {
        if (photonView.IsMine)
        {
            CheckButtonInput("ButtonFire", Elements.FIRE);
            CheckButtonInput("ButtonEarth", Elements.EARTH);
            CheckButtonInput("ButtonWater", Elements.WATER);
        }
        
    }

    private void CheckButtonInput(string buttonName, Elements element)
    {
        if (CrossPlatformInputManager.GetButtonDown(buttonName)) {
            elementController.CheckInputElement(spellElements, element);
            elementController.InitElement(spellElements);
            _gamemanager.SpellBase.GetNewSpell(spellElements, out Spell spell);
            joystickScr.ChangeSpell(spell);
        }
    }

    public void ClearElements()
    {
        spellElements.Clear();
        elementController.InitElement(spellElements);
    }
    
}
