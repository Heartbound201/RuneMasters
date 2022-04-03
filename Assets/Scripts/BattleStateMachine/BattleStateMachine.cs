    using System.Collections.Generic;
    using System.Linq;
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
        public List<EnemyUnit> enemies = new List<EnemyUnit>();

        public Party party;

        public Unit ActingUnit { get; set; }
        public HexTile<Tile> SelectedTile { get; set; }
        public Rune SelectedRune { get; set; }
        public Ability SelectedAbility { get; set; }
        public List<TileDirection> SelectedRuneSteps { get; set; }

        public TurnMenuController turnMenuController;
        public PartyInfoMenuController partyInfoMenuController;
        public GameOverPanelController gameOverPanelController;
        private void Start()
        {
            Debug.Log("Start game");
            // Start game in menu state
            ChangeState<UnitPlacementState>();
            EnemyUnit.KOEvent += OnEnemyUnitKoEvent;
        }

        private void OnEnemyUnitKoEvent(Unit unit)
        {
            enemies.Remove((EnemyUnit) unit);
            enemyPlans.FindAll(plan => plan.actor == unit).ForEach(plan => ClearAIPlanDangerFromTiles(plan, plan.attackLocation));
        }
        private void ClearAIPlanDangerFromTiles(AIPlan aiPlan, HexTile<Tile> aiPlanAttackLocation)
        {
            enemyPlans.Remove(aiPlan);
            if (aiPlan.ability == null) return;
            foreach (HexTile<Tile> tile in aiPlan.ability.abilityArea.GetTilesInArea(board, aiPlan.actor.tile, aiPlanAttackLocation))
            {
                tile.Data.SolveDanger(aiPlan);
            }
        }
        private void OnDestroy()
        {
            EnemyUnit.KOEvent -= OnEnemyUnitKoEvent;
        }

        protected internal void IsBattleOver()
        {
            if(party.health <= 0)
            {
                ChangeState<GameOverState>();
            }

            if (enemies.Count == 0 || enemies.TrueForAll(e => e.currentHealth <= 0))
            {
                ChangeState<VictoryState>();
            }
        }
        
    }