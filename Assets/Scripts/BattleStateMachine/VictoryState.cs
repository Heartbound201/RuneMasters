public class VictoryState : State
{
    public override void Enter()
    {
        base.Enter();
        owner.gameOverPanelController.ShowVictoryPanel();
    }
}