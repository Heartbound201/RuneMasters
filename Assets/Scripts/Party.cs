    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class Party
    {
        public int health;
        public int healthMax;
        public int mana;
        public int manaMax;
        public int manaReserve;
        public int manaReserveMax;

        public List<PlayerUnit> units = new List<PlayerUnit>();

        public int AvailableMana => mana + manaReserve;

        public void ResetTurn()
        {
            manaReserve = Mathf.Clamp(mana / 2, 0, manaReserveMax);
            mana = manaMax;
        }

        public void SpendMana(int amount)
        {
            if (manaReserve > 0)
            {
                int left = 0;
                if (amount > manaReserve)
                {
                    left = amount - manaReserve;
                }
                manaReserve = Mathf.Clamp(manaReserve - amount, 0, manaReserveMax);
                mana = Mathf.Clamp(mana - left, 0, manaMax);
            }
            else
            {
                mana = Mathf.Clamp(mana - amount, 0, manaMax);
            }
        }

        public void TakeDamage(int amount)
        {
            health = Mathf.Clamp(health - amount, 0, healthMax);
        }

        public void Heal(int amount)
        {
            health = Mathf.Clamp(health + amount, 0, healthMax);
        }
        
        public void Init()
        {
            health = healthMax;
            mana = manaMax;
            manaReserve = 0;
        }
    }