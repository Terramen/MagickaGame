/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    private float _speed;
    private int _damage;

    protected Skill(float speed, int damage)
    {
        _speed = speed;
        _damage = damage;
    }

    public int Damage
    {
        get => _damage;
        set => _damage = value;
    }

    public float Speed
    {
        get => _speed;
        set => _speed = value;
    }
    public abstract void Process();
}

public class Fireball : Skill
{
    public Fireball(float speed, int damage) : base(speed, damage)
    {
        speed = 50;
        damage = 
    }

    public override void Process()
    {
        Debug.Log("Fireball");
       // throw new System.NotImplementedException();
    }
}

public class Boulder : Skill
{
    public Boulder(float speed, int damage) : base(speed, damage)
    {
        
    }

    public override void Process()
    {
        Debug.Log("Boulder");
       // throw new System.NotImplementedException();
    }
}

public class SkillFactory
{
    public Skill GetSkill(string skillType)
    {
        switch (skillType)
        {
            case "fireball":
                return new Fireball();
            case "boulder":
                return new Boulder();
            default:
                return null;
        }
    }
}
*/
