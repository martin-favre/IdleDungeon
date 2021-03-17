using StateMachineCollection;

namespace GameManager
{
    internal class UpdatePointsState : State
    {
        private readonly IGameManager manager;

        public UpdatePointsState(IGameManager manager) {
            this.manager = manager;
        }
        //todo, have difficulty and points etc
        public override State OnDuring()
        {
            TerminateMachine();
            return new GenerateMapState(manager);
        }
    }
}