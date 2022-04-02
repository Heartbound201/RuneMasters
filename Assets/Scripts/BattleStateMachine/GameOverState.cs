public class GameOverState : State
{
    public override void Enter()
    {
        base.Enter();
        owner.gameOverPanelController.ShowDefeatPanel();
    }
}