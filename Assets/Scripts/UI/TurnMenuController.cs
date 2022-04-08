using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TurnMenuController : MonoBehaviour
{
    public static event Action<PlayerUnit> SelectUnit;

    public Camera camera;
    public BattleStateMachine stateMachine;

    public Transform charactersPanel;
    public Transform runesPanel;

    public TMP_Text movementInfo;
    public TMP_Text defenseInfo;

    public GameObject characterSelectionButton;
    public GameObject runeSelectionButton;
    public List<Sprite> costSprites = new List<Sprite>();

    private Party party;
    private List<RuneMenuItem> runeMenuItems = new List<RuneMenuItem>();


    public void Load(Party party)
    {
        this.party = party;
        foreach (PlayerUnit partyUnit in party.Units)
        {
            GameObject charaMenuGO = Instantiate(characterSelectionButton, charactersPanel);
            CharacterMenuItem characterMenuItem = charaMenuGO.GetComponent<CharacterMenuItem>();
            characterMenuItem.text.text = partyUnit.name;
            characterMenuItem.button.onClick.AddListener(() => SelectUnit?.Invoke(partyUnit));
        }
    }

    public void SelectCharacter(PlayerUnit unit)
    {
        stateMachine.ActingUnit = unit;

        CameraController.CameraLookAt(unit);
        // clear rune panel
        ClearRunePanel();

        // fill rune panel
        foreach (var rune in unit.runes)
        {
            GameObject runeMenuGO = Instantiate(runeSelectionButton, runesPanel);
            RuneMenuItem runeMenuItem = runeMenuGO.GetComponent<RuneMenuItem>();
            runeMenuItem.unit = unit;
            runeMenuItem.rune = rune;
            runeMenuItem.text.text = rune.RunePrototype.runeName;
            runeMenuItem.icon.sprite = rune.RunePrototype.icon;
            if(rune.RunePrototype.categorySprite)
            {
                runeMenuItem.categoryIcon.sprite = rune.RunePrototype.categorySprite;
            }
            if(costSprites != null && costSprites.Count >= rune.RunePrototype.Cost)
            {
                runeMenuItem.costIcon.sprite = costSprites[rune.RunePrototype.Cost];
            }
            runeMenuItem.button.onClick.AddListener(() => SelectRune(rune));
            if (!rune.IsAvailable(unit)) 
            {
                runeMenuItem.button.interactable = false;
            }
            runeMenuItems.Add(runeMenuItem);
        }

        ClearInfoPanel();

        FillInfoPanel(unit);
        
    }

    private void FillInfoPanel(PlayerUnit playerUnit)
    {
        movementInfo.text = $"{playerUnit.movement}/{playerUnit.movementMax}";
        defenseInfo.text = $"{playerUnit.defense}";
    }

    private void ClearInfoPanel()
    {
    }

    private void ClearRunePanel()
    {
        foreach (RuneMenuItem item in runeMenuItems)
        {
            Destroy(item.gameObject);
        }
        
        runeMenuItems.Clear();
    }

    private void SelectRune(Rune rune)
    {
        stateMachine.SelectedRune = rune;
        stateMachine.ChangeState<ActionSelectionState>();
        stateMachine.ChangeState<ConfirmRuneState>();
    }

}