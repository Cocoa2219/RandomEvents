using Exiled.API.Features;

namespace RandomEvents.API.Interfaces;

public interface IEvent
{
    public string Name { get; }
    public string DisplayName { get; }
    public string Description { get; }
    public void Run();

    public void RegisterEvents();
    public void UnregisterEvents();
}