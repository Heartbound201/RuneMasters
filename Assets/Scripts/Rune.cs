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

    public string Format(Unit unit)
    {
        return $"{RunePrototype.name}" +
               $"\n{RunePrototype.description}" + 
               (RunePrototype.cooldown > 0 ? $"\nCooldown: {RunePrototype.cooldown}" : "")+ 
               "\n" + (IsAvailable(unit) ? "Available" : $"Not available") + ((turnToWait > 0) ? $"( {turnToWait} Turns)" : "") + 
               $"\n{RunePrototype.ability.name}" +
               $"\n{RunePrototype.ability.description}";
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