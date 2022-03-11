    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Serialization;
    using Wunderwunsch.HexMapLibrary;
    using Wunderwunsch.HexMapLibrary.Generic;

    public class BattleStateMachine : StateMachine
    {
        public Board board;
        public TurnManager turnManager;
        public LevelData levelData;
        public List<AIPlan> enemyPlans = new List<AIPlan>();

        public Party party;
        public List<EnemyUnit> enemies = new List<EnemyUnit>();

        public Unit ActingUnit;
        public HexTile<Tile> SelectedTile;
        public Rune SelectedRune;
        public Ability SelectedAbility;
        public List<TileDirection> selectedRuneSteps;
        
        public TurnMenuController turnMenuController;
        public PartyInfoMenuController partyInfoMenuController;
        private void Start()
        {
            Debug.Log("Start game");
            // Start game in menu state
            ChangeState<UnitPlacementState>();
        }
        
    }