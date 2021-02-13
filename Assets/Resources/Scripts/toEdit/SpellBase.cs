using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
[CreateAssetMenu(fileName = "Element Base", menuName = "SpellBase")]
public class SpellBase : ScriptableObject
{

    [HideInInspector] public List<Spell> Spells;
    [SerializeField] private Spell currentSpell;
    private int currentIndex;

    #region SoInit
    public void CreateSpell()
    {
        if (Spells == null)
            Spells = new List<Spell>();
        Spell spell = new Spell();
        spell.init();
        Spells.Add(spell);
        spell.ID = Spells.Count;
        currentSpell = spell;
        currentIndex = Spells.Count - 1;
    }
    public void RemoveSpell()
    {
        Spells.Remove(currentSpell);
        if (Spells.Count > 0)
            currentSpell = Spells[0];
        else CreateSpell();
        currentIndex = 0;
    }

    public void NextSpell()
    {
        if (currentIndex + 1 < Spells.Count)
        {
            currentIndex++;
            currentSpell = Spells[currentIndex];
        }
    }
    public void PrevSpell()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            currentSpell = Spells[currentIndex];
        }
    }
    #endregion

    public bool GetNewSpell(List<Elements> spellElements, out Spell spell)
    {
       // CooldownManager.instance.SpellsOnCooldown.Count;
        spell = null;
        for (int i = 0; i < Spells.Count; i++)
        {
            if (Spells[i].SpellElements.SequenceEqual(spellElements))
            {
                spell = new Spell();
                spell.ID = Spells[i].ID;
                spell.spellPrefab = Spells[i].spellPrefab;
                spell.SpellIcon = Spells[i].SpellIcon;
                spell.spellName = "Skills/" + Spells[i].spellName;
                spell.castType = Spells[i].castType;
                spell.spellCooldown = Spells[i].spellCooldown;
                spell.currentSpellCooldown = Spells[i].currentSpellCooldown;

                spell.fillAmount = Spells[i].fillAmount;
                //Debug.Log(Spells[i].SpellIcon);
                return true;
            }
        }

        return false;
    }
}
[System.Serializable]
public class Spell
{
    [SerializeField] public string spellName;
    [SerializeField] public Sprite SpellIcon;
    [SerializeField] public GameObject spellPrefab;
    [SerializeField] public int ID;

    [SerializeField] public CastType castType;

    [SerializeField] public float spellCooldown;
    [SerializeField] public float currentSpellCooldown;

    [SerializeField] public float fillAmount;

    private Elements first;
    private Elements second;
    private Elements third;
    [SerializeField] private List<Elements> spellElements = new List<Elements>();

    public List<Elements> SpellElements
    {
        get => spellElements;
        set => spellElements = value;
    }

    public void init()
    {
        spellElements.Add(first);
        spellElements.Add(second);
        spellElements.Add(third);
    }

    public void GetSkillofID(int id)
    {
        if(id == 1)
        {
        }
    }
    public void InitSpell(Spell spell)
    {
      
    }

    public void PutOnCooldown()
    {
        CooldownManager.instance.StartCooldown(this);
    }
}
