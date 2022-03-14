public class DamageOvertimeStatus : Status
{
    public int potency;

    public override void ApplyOnTurnStart(Unit target)
    {
        base.ApplyOnTurnStart(target);
        target.TakeDamage(potency);
    }

    public DamageOvertimeStatus(int duration, int potency) : base(duration)
    {
        this.potency = potency;
    }
}