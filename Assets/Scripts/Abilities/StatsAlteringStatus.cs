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
}