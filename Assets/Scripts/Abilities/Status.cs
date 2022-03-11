public abstract class Status
{
    protected string name;
    protected int duration;

    protected Status(string name, int duration)
    {
        this.name = name;
        this.duration = duration;
    }

    public virtual void Apply(Unit target)
    {
        target.statuses.Add(this);
    }
    public virtual void ApplyOnTurnStart(Unit target){}

    public virtual void ApplyOnTurnEnd(Unit target)
    {
        duration--;
        if (duration <= 0)
        {
            Remove(target);
        }
    }

    public virtual void Remove(Unit target)
    {
        target.statuses.Remove(this);
    }

}