using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TurnMenuController : MonoBehaviour
{
    public static event Action<Unit> SelectUnit;

    public Camera camera;
    public BattleStateMachine stateMachine;

    public Transform charactersPanel;
    public Transform runesPanel;

    public TMP_Text movementInfo;

    public GameObject characterSelectionButton;
    public GameObject runeSelectionButton;

    private Party party;
    private List<RuneMenuItem> runeMenuItems = new List<RuneMenuItem>();

    public void Load(Party party)
    {
        this.party = party;
        foreach (PlayerUnit partyUnit in party.units)
        {
            GameObject charaMenuGO = Instantiate(characterSelectionButton, charactersPanel);
            CharacterMenuItem characterMenuItem = charaMenuGO.GetComponent<CharacterMenuItem>();
            characterMenuItem.text.text = partyUnit.name;
            characterMenuItem.button.onClick.AddListener(() => SelectCharacter(partyUnit));
        }
    }

    public void SelectCharacter(PlayerUnit unit)
    {
        stateMachine.ActingUnit = unit;
        
        float targetX = camera.transform.position.x - unit.transform.position.x;
        float targetZ = camera.transform.position.z - unit.transform.position.z;
        camera.transform.position -= new Vector3(targetX, 0, targetZ);

        // clear rune panel
        ClearRunePanel();

        // fill rune panel
        foreach (Rune runePrototype in unit.runes)
        {
            GameObject runeMenuGO = Instantiate(runeSelectionButton, runesPanel);
            RuneMenuItem runeMenuItem = runeMenuGO.GetComponent<RuneMenuItem>();
            runeMenuItem.rune = runePrototype;
            runeMenuItem.text.text = runePrototype.runeName;
            runeMenuItem.icon.sprite = runePrototype.icon;
            runeMenuItem.button.onClick.AddListener(() => SelectRune(runePrototype));
            if (unit.hasActed || party.AvailableMana < runePrototype.steps.Count) 
            {
                runeMenuItem.button.interactable = false;
            }
            runeMenuItems.Add(runeMenuItem);
        }

        ClearInfoPanel();

        FillInfoPanel(unit);
        
        // mov / str / dex / int / def
        // 3     3     3      2     4
        // 
        // unit.statuses[0]
        // unit.strength <= unit.strengthStart;

        //TODO gray out commands

        SelectUnit?.Invoke(unit);
    }

    private void FillInfoPanel(PlayerUnit playerUnit)
    {
        movementInfo.text = $"{playerUnit.movement}/{playerUnit.movementMax}";
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