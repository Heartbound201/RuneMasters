public class StrengthAlteringStatus : Status
{
    public int amount;


    public StrengthAlteringStatus(int amount, string name, int duration) : base(name, duration)
    {
        this.amount = amount;
    }

    public override void Apply(Unit target)
    {
        base.Apply(target);
        target.strength += amount;
    }

    public override void Remove(Unit target)
    {
        base.Remove(target);
        target.strength -= amount;
    }
}