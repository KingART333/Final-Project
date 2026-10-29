using Assets._Project.Develop.Runtime.Utilities.Reactive;

namespace Assets._Project.Develop.Runtime.Utilities.StateMachineCore
{
    public interface IState
    {
        IReadonlyEvent Entered {  get; }
        IReadonlyEvent Exited { get; }

        void Enter();
        void Exit();
    }
}
