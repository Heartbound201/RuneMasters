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


        public void Reset()
        {
            health = healthMax;
            mana = manaMax;
            manaReserve = 0;
        }
    }