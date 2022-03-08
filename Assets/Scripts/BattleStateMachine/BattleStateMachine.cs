    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Serialization;
    using Wunderwunsch.HexMapLibrary.Generic;

    public class BattleStateMachine : StateMachine
    {
        public Board board;
        public TurnManager turnManager;
        public LevelData levelData;
        public AIController aiController;

        public Party party;
        public List<Unit> enemies = new List<Unit>();

        public Unit ActingUnit;
        public HexTile<Tile> SelectedTile;
        public RunePrototype SelectedRune;
        public Ability SelectedAbility;

        public TurnMenuController turnMenuController;

        private void Start()
        {
            Debug.Log("Start game");
            // Start game in menu state
            ChangeState<UnitPlacementState>();
        }
        
    }