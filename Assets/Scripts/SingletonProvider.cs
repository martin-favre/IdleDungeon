using GameManager;
using PubSubSystem;

public static class SingletonProvider
{
    private static ICombatManager combatManager = CombatManager.Instance;
    public static ICombatManager MainCombatManager { get => combatManager; set => combatManager = value; }
    private static IPlayerRoster playerRoster = PlayerRoster.Instance;
    public static IPlayerRoster MainPlayerRoster { get => playerRoster; set => playerRoster = value; }
    private static IGameManager gameManager = GameManager.GameManager.Instance;
    public static IGameManager MainGameManager
    {
        get
        {
            if (gameManager == null) gameManager = GameManager.GameManager.Instance;
            return gameManager;
        }
        set => gameManager = value;
    }
    private static IRandomProvider randomProvider = SystemRandom.Instance;
    public static IRandomProvider MainRandomProvider
    {
        get
        {
            if (randomProvider == null) randomProvider = SystemRandom.Instance;
            return randomProvider;
        }
        set => randomProvider = value;
    }
    private static IPersistentDataStorage dataStorage = PlayerPrefsReader.Instance;
    public static IPersistentDataStorage MainDataStorage
    {
        get
        {
            if (dataStorage == null) dataStorage = PlayerPrefsReader.Instance;
            return dataStorage;
        }
        set => dataStorage = value;
    }
    private static ITimeProvider timeProvider = UnityTime.Instance;
    public static ITimeProvider MainTimeProvider { get => timeProvider; set => timeProvider = value; }
    private static IPlayerWallet playerWallet = PlayerWallet.Instance;
    public static IPlayerWallet MainPlayerWallet { get => playerWallet; set => playerWallet = value; }
    private static IGuidProvider guidProvider = GuidProvider.Instance;
    public static IGuidProvider MainGuidProvider { get => guidProvider; set => guidProvider = value; }
    private static IGameObjectLoader prefabLoader = PrefabLoader.Instance;
    public static IGameObjectLoader MainGameobjectLoader
    {
        get
        {
            if (prefabLoader == null) prefabLoader = PrefabLoader.Instance;
            return prefabLoader;
        }
        set => prefabLoader = value;
    }
    private static IMovementController playerController = MovementController.Instance;
    public static IMovementController MainPlayerController { get => playerController; set => playerController = value; }
    private static IUpgradeManager upgradeManager = UpgradeManager.Instance;
    public static IUpgradeManager MainUpgradeManager
    {
        get
        {
            if (upgradeManager == null) upgradeManager = UpgradeManager.Instance;
            return upgradeManager;
        }
        set => upgradeManager = value;
    }
    private static IEventPublisher<EventType> mainEventPublisher = CentralEventPublisher.Instance;
    public static IEventPublisher<EventType> MainEventPublisher
    {
        get
        {
            if (mainEventPublisher == null) mainEventPublisher = CentralEventPublisher.Instance;
            return mainEventPublisher;
        }
        set => mainEventPublisher = value;
    }


}