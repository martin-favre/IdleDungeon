public class CombatResult
{
    private readonly int gainedGold;
    private readonly bool playerWon;

    public CombatResult(int gainedGold, bool playerWon)
    {
        this.gainedGold = gainedGold;
        this.playerWon = playerWon;
    }

    public int GainedGold => gainedGold;

    public bool PlayerWon => playerWon;
}