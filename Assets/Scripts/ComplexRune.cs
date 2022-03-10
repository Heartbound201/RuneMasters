    using System.Collections.Generic;
    using UnityEngine;
    using Wunderwunsch.HexMapLibrary;

    public class ComplexRune : ScriptableObject
    {
        public string name;
        public List<List<TileDirection>> stepsList = new List<List<TileDirection>>();
        public Ability ability;
    }