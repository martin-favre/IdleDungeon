namespace GameManager
{
    public interface IGameManager
    {
        IMap GridMap { get; set; }
        IMapFactory MapFactory { get; }
        IMapModifier[] MapModifiers { get; }
        void SpawnMap();
        void SpawnPlayer();
        void FadeOut();
        void FadeIn();
    }
}