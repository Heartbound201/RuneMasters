    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Wunderwunsch.HexMapLibrary.Generic;

    public class PlayerUnit : Unit
    {
        public List<Rune> runes = new List<Rune>();

        public Party party;
        
        public override bool ExpandSearch(HexTile<Tile> from, HexTile<Tile> to)
        {
            return (from.Data._distance + 1) <= movement && (from.Data._distance + 1) <= party.AvailableMana;
        }
        
        public override IEnumerator Move(List<HexTile<Tile>> tiles)
        {
            foreach (HexTile<Tile> tile in tiles)
            {
                transform.position = tile.CartesianPosition;
                base.tile.Data.unit = null;
                base.tile = tile;
                tile.Data.unit = this;
                
                movement--;
                party.mana--;
                yield return new WaitForSeconds(0.5f);
            }
        }

        public override void TakeDamage(int amount)
        {
            base.TakeDamage(amount);
            party.health -= (amount - defense);
            Debug.Log(name + " is hit for " + (amount - defense));
        }
    }