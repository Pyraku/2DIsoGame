using System;

public abstract class State_Base<T>
    where T : Enum
{
    public abstract T State { get; }

    protected ManagerBase<T> Manager { get; private set; } = null;

    public State_Base(ManagerBase<T> m) { Manager = m; }

    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
}
