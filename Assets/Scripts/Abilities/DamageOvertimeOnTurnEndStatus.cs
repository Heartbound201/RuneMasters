public class DamageOvertimeOnTurnEndStatus: Status
{
    public int potency;

    public override void ApplyOnTurnEnd(Unit target)
    {
        base.ApplyOnTurnEnd(target);
        target.TakeDamage(potency);
    }

    public DamageOvertimeOnTurnEndStatus(int duration, int potency) : base(duration)
    {
        this.potency = potency;
    }
    
    public override string Summary()
    {
        string text = $"<b>{potency}</b> Dot Dmg ({duration} turns)";
        return text;
    }
}