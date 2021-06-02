using StateMachineCollection;

namespace GameManager
{
    public class UpdatePointsState : State
    {
        private readonly IGameManager manager;
        private readonly bool playerDied;

        public UpdatePointsState(IGameManager manager, bool playerDied)
        {
            this.manager = manager;
            this.playerDied = playerDied;
        }

        public override void OnEntry()
        {
            var currentLevel = SingletonProvider.MainDataStorage.GetInt(Constants.currentLevelKey, 0);
            int increment = playerDied ? -1 : 1;
            var nextLevel = currentLevel + increment;
            if (nextLevel < 0)
            {
                nextLevel = 0;
            }
            SingletonProvider.MainDataStorage.SetInt(Constants.currentLevelKey, nextLevel);
        }

        public override State OnDuring()
        {
            TerminateMachine();
            return new GenerateMapState(manager);
        }
    }
}