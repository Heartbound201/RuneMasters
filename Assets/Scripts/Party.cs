    using System;
    using System.Collections.Generic;
    using UnityEngine;

    [CreateAssetMenu(fileName = "Party", menuName = "Create Party SO")]
    public class Party : ScriptableObject
    {
        public int health;
        public int healthMax;
        public int mana;
        public int manaMax;
        public int manaReserve;
        public int manaReserveMax;

        public List<Unit> units = new List<Unit>();

        public int AvailableMana => mana + manaReserve;

        public void Reset()
        {
            manaReserve = Mathf.Clamp(mana / 2, 0, manaReserveMax);
            mana = manaMax;
        }
        
        

    }