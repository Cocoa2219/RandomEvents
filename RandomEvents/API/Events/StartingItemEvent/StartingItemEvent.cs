using System;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using MEC;
using Random = UnityEngine.Random;

namespace RandomEvents.API.Events.StartingItemEvent;

public class StartingItemEvent : Event
{
    public override string Name { get; } = "StartingItemEvent";
    public override string DisplayName { get; } = "시작 아이템";
    public override string Description { get; } = "스폰 시 랜덤한 인벤토리를 가집니다.";

    private ItemType GetRandomItem()
    {
        var itemTypes = Enum.GetValues(typeof(ItemType));
        ItemType randomItemType;
        do
        {
            // Random.InitState((int) (Time.time * 1000));
            randomItemType = (ItemType)itemTypes.GetValue(Random.Range(0, itemTypes.Length));
        } while (randomItemType == ItemType.None);

        return randomItemType;
    }

    public override void Run()
    {
        Timing.CallDelayed(0.1f, () =>
        {
            foreach (var player in Player.List)
            {
                if (player.IsScp || !player.IsAlive) continue;

                for (var i = 0; i < Random.Range(5, 8); i++) player.AddItem(GetRandomItem());
            }
        });
    }

    public override void RegisterEvents()
    {
        Exiled.Events.Handlers.Player.ChangingRole += OnRoleChanging;
    }

    public override void UnregisterEvents()
    {
        Exiled.Events.Handlers.Player.ChangingRole -= OnRoleChanging;
    }

    private void OnRoleChanging(ChangingRoleEventArgs ev)
    {
        Timing.CallDelayed(.1f, () =>
        {
            if (ev.Player.IsScp || !ev.Player.IsAlive) return;

            ev.Player.ClearInventory();
            for (var i = 0; i < Random.Range(5, 8); i++) ev.Player.AddItem(GetRandomItem());
        });
    }
}