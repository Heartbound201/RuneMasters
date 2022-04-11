using System.Collections.Generic;
using UnityEngine;

public class Party : MonoSingleton<Party>
{
    [field: SerializeField] public int Health { get; set; }
    [field: SerializeField] public int HealthMax { get; set; }
    [field: SerializeField] public int Mana { get; set; }
    [field: SerializeField] public int ManaMax { get; set; }
    [field: SerializeField] public int ManaReserve { get; set; }
    [field: SerializeField] public int ManaReserveMax { get; set; }

    public List<PlayerUnit> Units { get; } = new List<PlayerUnit>();

    public int AvailableMana => Mana + ManaReserve;

    public void ResetTurn()
    {
        ManaReserve = Mathf.Clamp(Mana / 2, 0, ManaReserveMax);
        Mana = ManaMax;
    }

    public void SpendMana(int amount)
    {
        if (ManaReserve > 0)
        {
            int left = 0;
            if (amount > ManaReserve)
            {
                left = amount - ManaReserve;
            }

            ManaReserve = Mathf.Clamp(ManaReserve - amount, 0, ManaReserveMax);
            Mana = Mathf.Clamp(Mana - left, 0, ManaMax);
        }
        else
        {
            Mana = Mathf.Clamp(Mana - amount, 0, ManaMax);
        }
    }

    public void FillMana(int amount)
    {
        int left = (Mana + amount) - ManaMax;
        Mana = Mathf.Clamp(Mana + amount, 0, ManaMax);
        if (left > 0)
        {
            ManaReserve = Mathf.Clamp(ManaReserve + left, 0, ManaReserveMax);
        }
    }

    public void TakeDamage(int amount)
    {
        Health = Mathf.Clamp(Health - amount, 0, HealthMax);
    }

    public void Heal(int amount)
    {
        Health = Mathf.Clamp(Health + amount, 0, HealthMax);
    }

    public void Init()
    {
        Health = HealthMax;
        Mana = ManaMax;
        ManaReserve = 0;
    }
}