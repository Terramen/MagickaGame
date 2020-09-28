using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill
{
    public abstract void Process();
}

public class Fireball : Skill
{
    public override void Process()
    {
        Debug.Log("Fireball");
       // throw new System.NotImplementedException();
    }
}

public class Boulder : Skill
{
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
