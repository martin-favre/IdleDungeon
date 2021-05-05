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
            var currentLevel = manager.DataStorage?.GetInt(Constants.currentLevelKey, 0);
            if (currentLevel.HasValue)
            {
                int increment = playerDied ? -1 : 1;
                var nextLevel = currentLevel.Value + increment;
                if (nextLevel < 0)
                {
                    nextLevel = 0;
                }
                this.manager.DataStorage?.SetInt(Constants.currentLevelKey, nextLevel);
            }

            if (playerDied)
            {
                foreach (var character in manager.PlayerChars.GetAllPlayersChars())
                {
                    character.Attributes.Heal(character.Attributes.MaxHp);
                }
            }
        }

        public override State OnDuring()
        {
            TerminateMachine();
            return new GenerateMapState(manager);
        }
    }
}