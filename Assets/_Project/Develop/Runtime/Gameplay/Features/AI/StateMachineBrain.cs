namespace Assets._Project.Develop.Runtime.Gameplay.Features.AI
{
    public class StateMachineBrain : IBrain
    {
        private AiStateMachine _stateMachine;

        private bool _isEnabled;

        public StateMachineBrain(AiStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public void Disable()
        {
            _stateMachine.Exit();
            _isEnabled = false;
        }

        public void Dispose()
        {
            _stateMachine.Dispose();
            _isEnabled = false;
        }

        public void Enable()
        {
            _stateMachine.Enter();
            _isEnabled = true;
        }

        public void Update(float deltaTime)
        {
            if (_isEnabled == false)
                return;

            _stateMachine.Update(deltaTime);
        }
    }
}
