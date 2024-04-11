using Exiled.API.Features;
using RandomEvents.API.Interfaces;
using Player = Exiled.Events.Handlers.Player;

namespace RandomEvents.API.Events.GamblingEvent;

public class GamblingEvent: Event
{
    public override string Name { get; } = "GamblingEvent";
    public override string DisplayName { get; } = "도박";
    public override string Description { get; } = "아이템을 떨어트릴 시 2% 확률로 손이 잘리거나 랜덤한 아이템을 획득합니다.";

    private GamblingEventHandler EventHandler { get; set; }

    public override void Run()
    {
        EventHandler = new GamblingEventHandler(this);
    }

    public override void RegisterEvents()
    {
        Player.DroppedItem += EventHandler.OnDroppedItem;
    }

    public override void UnregisterEvents()
    {
        Player.DroppedItem -= EventHandler.OnDroppedItem;
    }
}