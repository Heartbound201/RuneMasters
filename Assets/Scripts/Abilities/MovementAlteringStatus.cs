public class MovementAlteringStatus : Status
{
    public int movement;
    public MovementAlteringStatus(int duration,  int movement) : base(duration)
    {
        this.movement = movement;
    }
    public override void Apply(Unit target)
    {
        base.Apply(target);
        target.movementMax += movement;
    }

    public override void Remove(Unit target)
    {
        base.Remove(target);

        target.movementMax -= movement;
    }
    public override string Summary()
    {
        string text = $"<b>{movement}</b> Movement for {duration} turns";
        return text;
    }
}