namespace GameManager
{
    public interface IGameManager
    {
        IGridMap GridMap { get; set; }
        IMazeFactory MapFactory { get; }
        IMazeModifier[] MapModifiers { get; }

        void SpawnMaze();
        void SpawnPlayer();
    }
}