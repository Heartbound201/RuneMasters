    using System.Collections.Generic;
    using System.Linq;
    using TMPro;
    using UnityEngine;
    using UnityEngine.Playables;
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
        public List<HexTile<Tile>> SelectedRuneSteps { get; set; }

        public TurnMenuController turnMenuController;
        public PartyInfoMenuController partyInfoMenuController;
        public GameOverPanelController gameOverPanelController;
        public PauseController pauseController;
        public TMP_Text stateHintText;

        public PlayableDirector HudFadeInTimeline;
        
        [Header("Audio")] public AudioClipSO battleTheme;
        private void Start()
        {
            Debug.Log("Start game");
            if(battleTheme) AudioManager.Instance.PlayMusic(battleTheme);
            // Start game in menu state
            ChangeState<UnitPlacementState>();
            EnemyUnit.KOEvent += OnEnemyUnitKoEvent;
        }

        public void ShowHint(string text)
        {
            stateHintText.text = text;
            stateHintText.transform.parent.gameObject.SetActive(true);
        }
        public void HideHint()
        {
            stateHintText.transform.parent.gameObject.SetActive(false);
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
            if(party.Health <= 0)
            {
                ChangeState<GameOverState>();
            }

            if (enemies.Count == 0 || enemies.TrueForAll(e => e.currentHealth <= 0))
            {
                ChangeState<VictoryState>();
            }
        }

        public void PauseOrResumeGame()
        {
            if(pauseController != null)
            {
                pauseController.PauseOrResume();
            }
        }
        
    }