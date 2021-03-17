namespace GameManager
{
    public interface IGameManager
    {
        IGridMap GridMap { get; set; }
        IMazeGenerator MapGenerator { get; set; }

        void SpawnMaze();
        void SpawnPlayer();
    }
}