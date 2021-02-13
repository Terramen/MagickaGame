using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CooldownManager : MonoBehaviour
{
    public static CooldownManager instance;
    [SerializeField] private List<Spell> spellsOnCooldown;

    public List<Spell> SpellsOnCooldown
    {
        get => spellsOnCooldown;
        set => spellsOnCooldown = value;
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(this);
        }
        if (spellsOnCooldown == null)
        {
            spellsOnCooldown = new List<Spell>();
        }
    }

    void Update()
    {
        for (int i = 0; i < spellsOnCooldown.Count; i++)
        {
            spellsOnCooldown[i].currentSpellCooldown -= Time.deltaTime;

            if (spellsOnCooldown[i].currentSpellCooldown <= 0)
            {
                spellsOnCooldown[i].currentSpellCooldown = 0;
                spellsOnCooldown.Remove(spellsOnCooldown[i]);
            }
        }
    }

    public void StartCooldown(Spell spell)
    {
        if (!spellsOnCooldown.Contains(spell))
        {
            spell.currentSpellCooldown = spell.spellCooldown;
            spellsOnCooldown.Add(spell);
        }
    }

    public bool CheckSpell(int ID)
    {
        for (int i = 0; i < spellsOnCooldown.Count; i++)
        {
            if (ID == spellsOnCooldown[i].ID)
            {
                return true;
            }
        }

        return false;
    }

    public bool TryGetCurrentCooldown(int ID, out float currentCooldown)
    {
        currentCooldown = 0f;
        for (int i = 0; i < spellsOnCooldown.Count; i++)
        {
            if (ID == spellsOnCooldown[i].ID)
            {
                currentCooldown = spellsOnCooldown[i].currentSpellCooldown;
                return true;
            }
        }

        return false;
    }

    public bool TryGetSpell(int ID, out Spell spell)
    {
        spell = null;
        for (int i = 0; i < spellsOnCooldown.Count; i++)
        {
            if (ID == spellsOnCooldown[i].ID)
            {
                spell = spellsOnCooldown[i];
                return true;
            }
        }

        return false;
    }
}
