using Exiled.API.Features;
using RandomEvents.API.Interfaces;
using Player = Exiled.Events.Handlers.Player;

namespace RandomEvents.API.Events.GamblingEvent;

public class GamblingEvent: IEvent
{
    public void LogInfo(object message)
    {
        Log.Info($"({Name}) {message}");
    }

    public void LogDebug(object message)
    {
        Log.Debug($"({Name}) {message}");
    }

    public void LogWarn(object message)
    {
        Log.Warn($"({Name}) {message}");
    }

    public void LogError(object message)
    {
        Log.Error($"({Name}) {message}");
    }


    public string Name { get; } = "GamblingEvent";
    public string DisplayName { get; } = "도박";
    public string Description { get; } = "아이템을 떨어트릴 시 2% 확률로 손이 잘리거나 랜덤한 아이템을 획득합니다.";

    private GamblingEventHandler EventHandler { get; set; }

    public void Run()
    {
        EventHandler = new GamblingEventHandler(this);
    }

    public void RegisterEvents()
    {
        Player.DroppedItem += EventHandler.OnDroppedItem;
    }

    public void UnregisterEvents()
    {
        Player.DroppedItem -= EventHandler.OnDroppedItem;
    }
}