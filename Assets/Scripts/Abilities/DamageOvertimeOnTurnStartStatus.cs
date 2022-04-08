using System;
using System.Collections.Generic;

public class DamageOvertimeOnTurnStartStatus : Status
{
    public int potency;

    public override void ApplyOnTurnStart(Unit target)
    {
        base.ApplyOnTurnStart(target);
        target.TakeDamage(potency);
    }

    public DamageOvertimeOnTurnStartStatus(int duration, int potency) : base(duration)
    {
        this.potency = potency;
    }
    
    public override string Summary()
    {
        string text = $"<b>{potency}</b> Damage Overtime for {duration} turns";
        return text;
    }
}