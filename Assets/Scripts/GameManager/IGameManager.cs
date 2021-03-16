namespace GameManager
{
    public interface IGameManager
    {
        IGridMap GridMap { get; set; }
        IMaceGenerator MapGenerator { get; set; }

        void SpawnMaze();
        void SpawnPlayer();
    }
}