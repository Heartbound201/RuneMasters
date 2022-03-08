using UnityEngine;

public class TurnMenuController : MonoBehaviour
{
    public Party party;
    public BattleStateMachine stateMachine;
    
    public Transform charactersPanel;
    public Transform infoPanel;
    public Transform runesPanel;

    public GameObject characterSelectionButton;
    public GameObject runeSelectionButton;

    public void Load()
    {
        foreach (Unit partyUnit in party.units)
        {
            GameObject charaMenuGO = Instantiate(characterSelectionButton, charactersPanel);
            CharacterMenuItem characterMenuItem = charaMenuGO.GetComponent<CharacterMenuItem>();
            characterMenuItem.text.text = partyUnit.name;
            characterMenuItem.button.onClick.AddListener(() => SelectCharacter(partyUnit));
        }
    }

    public void SelectCharacter(Unit partyUnit)
    {
        stateMachine.ActingUnit = partyUnit;
        //TODO center camera
        
        // clear rune panel
        for (int i = 0; i < runesPanel.childCount; i++)
        {
            Destroy(runesPanel.GetChild(i).gameObject);
        }
        
        // fill rune panel
        foreach (RunePrototype runePrototype in partyUnit.runes)
        {
            GameObject runeMenuGO = Instantiate(runeSelectionButton, runesPanel);
            RuneMenuItem runeMenuItem = runeMenuGO.GetComponent<RuneMenuItem>();
            runeMenuItem.runePrototype = runePrototype;
            runeMenuItem.text.text = runePrototype.runeName;
            runeMenuItem.icon.sprite = runePrototype.icon;
            runeMenuItem.button.onClick.AddListener(() => SelectRune(runePrototype));
            
        }
        
        //TODO clear info panel
        
        //TODO fill info panel
        
        //TODO gray out commands
    }

    private void SelectRune(RunePrototype runePrototype)
    {
        stateMachine.SelectedRune = runePrototype;
        stateMachine.ChangeState<ConfirmRuneState>();
    }
}