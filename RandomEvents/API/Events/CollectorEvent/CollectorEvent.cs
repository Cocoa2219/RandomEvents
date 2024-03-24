using System;
using System.Collections.Generic;
using System.Linq;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using MEC;
using RandomEvents.API.Interfaces;
using UnityEngine;
using Random = UnityEngine.Random;

namespace RandomEvents.API.Events.CollectorEvent;

public class CollectorEvent : IEvent
{
    public string Name { get; } = "CollectorEvent";
    public string DisplayName { get; } = "수집가";
    public string Description { get; } = "랜덤한 SCP 아이템 1~3개를 들고 시작합니다.";

    public ItemType GetRandomScpItem()
    {
        Array scpItemTypes = Enum.GetValues(typeof(ItemType))
            .Cast<ItemType>()
            .Where(IsScpItem)
            .ToArray();

        ItemType randomSCPItem = (ItemType)scpItemTypes.GetValue(Random.Range(0, scpItemTypes.Length));
        return randomSCPItem;
    }

    private bool IsScpItem(ItemType item)
    {
        return item.ToString().StartsWith("SCP");
    }

    public void Run()
    {
        Timing.CallDelayed(0.1f, () =>
        {
            foreach (var player in Player.List)
            {
                if (player.IsScp || !player.IsAlive) continue;

                // Random.InitState((int)(Time.time * 1000));
                for (var i = 0; i < Random.Range(1, 4); i++)
                {
                    player.AddItem(GetRandomScpItem());
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

            for (var i = 0; i < Random.Range(1, 4); i++)
            {
                ev.Player.AddItem(GetRandomScpItem());
            }
        });
    }
}