using System;
using System.Collections.Generic;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using MEC;
using RandomEvents.API.Interfaces;
using UnityEngine;
using Random = UnityEngine.Random;

namespace RandomEvents.API.Events.StartingItemEvent;

public class StartingItemEvent : IEvent
{
    public string Name { get; } = "StartingItemEvent";
    public string DisplayName { get; } = "시작 아이템";
    public string Description { get; } = "스폰 시 랜덤한 인벤토리를 가집니다.";

    private ItemType GetRandomItem()
    {
        var itemTypes = Enum.GetValues(typeof(ItemType));
        ItemType randomItemType;
        do
        {
            // Random.InitState((int) (Time.time * 1000));
            randomItemType = (ItemType) itemTypes.GetValue(Random.Range(0, itemTypes.Length));
        } while (randomItemType == ItemType.None);

        return randomItemType;
    }

    public void Run()
    {
        Timing.CallDelayed(0.1f, () =>
        {
            foreach (var player in Player.List)
            {
                if (player.IsScp || !player.IsAlive) continue;

                Random.InitState((int) (Time.time * 1000));
                for (var i = 0; i < Random.Range(5, 8); i++)
                {
                    player.AddItem(GetRandomItem());
                }
            }
        });
    }

    public void RegisterEvents()
    {
        Exiled.Events.Handlers.Player.ChangingRole += OnRoleChanging;
    }

    public void UnregisterEvents()
    {
        Exiled.Events.Handlers.Player.ChangingRole -= OnRoleChanging;
    }

    private void OnRoleChanging(ChangingRoleEventArgs ev)
    {
        Timing.CallDelayed(.1f, () =>
        {
            if (ev.Player.IsScp || !ev.Player.IsAlive) return;

            ev.Player.ClearInventory();
            Random.InitState((int) (Time.time * 1000));
            for (var i = 0; i < Random.Range(5, 8); i++)
            {
                ev.Player.AddItem(GetRandomItem());
            }
        });
    }
}