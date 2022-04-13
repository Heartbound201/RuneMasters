using System;
using UnityEngine;

public class Rune
{
    public RunePrototype RunePrototype { get; }
    private int turnToWait = 0;

    public Rune(RunePrototype runePrototype)
    {
        this.RunePrototype = runePrototype;
    }

    public bool IsAvailable(Unit unit)
    {
        if (unit == null) return false;
        if (turnToWait > 0) return false;
        if (unit.hasActed) return false;
        if (unit.AvailableMana() < RunePrototype.Cost) return false;
        return true;
    }

    public string Summary(Unit unit)
    {
        string cooldownText = "";
        if (RunePrototype.cooldown > 0)
        {
            cooldownText += $"\n";
            if (IsAvailable(unit))
            {
                cooldownText += $"\nCooldown: {RunePrototype.cooldown} turns";
            }
            else
            {
                cooldownText += $"Unavailable for {turnToWait} turns";
            }
        }

        return $"<b>{RunePrototype.runeName}</b>" +
               $"\nCategory: {RunePrototype.category}" +
               $"\nMana Cost: <b>{RunePrototype.Cost}</b>" +
               $"\n{RunePrototype.ability.Summary()}" +
               $"{cooldownText}";

    }


    public void SendInCooldown()
    {
        turnToWait = RunePrototype.cooldown + 1;
    }

    public void LowerCooldown()
    {
        turnToWait = Mathf.Clamp(turnToWait - 1, 0, turnToWait);
    }
}