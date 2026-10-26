using System;

namespace Assets._Project.Develop.Runtime.Utilities.Reactive
{
    public interface IReadonlyEvent
    {
        IDisposable Subscribe(Action action);
    }

    public interface IreadonlyEvent<T>
    {
        IDisposable Subscribe(Action<T> action);
    }

}
