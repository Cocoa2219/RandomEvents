using Exiled.Events.EventArgs.Player;
using Exiled.Events.Handlers;
using RandomEvents.API.Interfaces;

namespace RandomEvents.API.Events.OneKillEvent;

public class OneKillEvent : Event
{
    public override string Name { get; } = "OneKillEvent";
    public override string DisplayName { get; } = "한 방";
    public override string Description { get; } = "0.1의 데미지만 받더라도 즉사합니다.";
    public override void Run()
    {

    }

    public override void RegisterEvents()
    {
        Player.Hurting += OnPlayerHurting;
    }

    public override void UnregisterEvents()
    {
        Player.Hurting -= OnPlayerHurting;
    }

    private void OnPlayerHurting(HurtingEventArgs ev)
    {
        ev.Amount = -1f;
    }
}