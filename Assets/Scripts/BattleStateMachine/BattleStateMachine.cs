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
        public GameObject defeatPanel;
        public GameObject victoryPanel;
        private void Start()
        {
            Debug.Log("Start game");
            // Start game in menu state
            ChangeState<UnitPlacementState>();
        }

        protected internal void IsBattleOver()
        {
            if(party.health <= 0)
            {
                ChangeState<GameOverState>();
            }

            if (enemies.TrueForAll(e => e.currentHealth <= 0))
            {
                ChangeState<VictoryState>();
            }
        }
        
    }

    public class VictoryState : State
    {
        public override void Enter()
        {
            base.Enter();
            owner.victoryPanel.SetActive(true);
        }
    }

    public class GameOverState : State
    {
        public override void Enter()
        {
            base.Enter();
            owner.defeatPanel.SetActive(true);
        }
    }