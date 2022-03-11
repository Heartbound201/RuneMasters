public class AttackAlteringStatus : Status
{
    public int amount;


    public AttackAlteringStatus(int amount, string name, int duration) : base(name, duration)
    {
        this.amount = amount;
    }

    public override void Apply(Unit target)
    {
        base.Apply(target);
        target.attack += amount;
    }

    public override void Remove(Unit target)
    {
        base.Remove(target);
        target.attack -= amount;
    }
}