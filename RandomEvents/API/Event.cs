using Exiled.API.Features;
using RandomEvents.API.Interfaces;

namespace RandomEvents.API;

public abstract class Event : IEvent
{
    public abstract string Name { get; }
    public abstract string DisplayName { get; }
    public abstract string Description { get; }
    public abstract void Run();

    public abstract void RegisterEvents();

    public abstract void UnregisterEvents();

    public void LogDebug(object obj)
    {
        Log.Debug($"({Name}) {obj}");
    }

    public void LogInfo(object obj)
    {
        Log.Info($"({Name}) {obj}");
    }

    public void LogWarn(object obj)
    {
        Log.Warn($"({Name}) {obj}");
    }

    public void LogError(object obj)
    {
        Log.Error($"({Name}) {obj}");
    }
}