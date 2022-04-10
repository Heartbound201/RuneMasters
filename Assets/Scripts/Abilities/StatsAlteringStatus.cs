using System;
using System.Collections.Generic;

public class StatsAlteringStatus : Status
{
    public int strength;
    public int defense;
    public int intelligence;
    public int dexterity;

    public StatsAlteringStatus(int duration, int strength, int defense, int intelligence, int dexterity) :
        base(duration)
    {
        this.strength = strength;
        this.defense = defense;
        this.intelligence = intelligence;
        this.dexterity = dexterity;
    }

    public override void Apply(Unit target)
    {
        base.Apply(target);
        target.strength += strength;
        target.defense += defense;
        target.intelligence += intelligence;
        target.dexterity += dexterity;
    }

    public override void Remove(Unit target)
    {
        base.Remove(target);

        target.strength -= strength;
        target.defense -= defense;
        target.intelligence -= intelligence;
        target.dexterity -= dexterity;
    }
    
    public override string Summary()
    {
        List<string> stats = new List<string>();
        
        if (strength != 0)
        {
            stats.Add($"{strength:+#;-#;0} STR");
        }
        if (defense != 0)
        {
            stats.Add($"{defense:+#;-#;0} DEF");
        }
        if (intelligence != 0)
        {
            stats.Add($"{intelligence:+#;-#;0} INT");
        }
        if (dexterity != 0)
        {
            stats.Add($"{dexterity:+#;-#;0} DEX");
        }

        string statsString = String.Join(", ", stats);
        string text = $"<b>{statsString}</b> for {duration} turns";
        return text;
    }
}