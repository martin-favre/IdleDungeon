using StateMachineCollection;

namespace GameManager
{
    public class UpdatePointsState : State
    {
        private readonly IGameManager manager;

        public UpdatePointsState(IGameManager manager)
        {
            this.manager = manager;
        }

        public override void OnEntry()
        {
            var currentLevel = manager.DataStorage.GetInt("currentLevel", 0);
            this.manager.DataStorage.SetInt("currentLevel", currentLevel + 1);
        }

        //todo, have difficulty and points etc
        public override State OnDuring()
        {
            TerminateMachine();
            return new GenerateMapState(manager);
        }
    }
}