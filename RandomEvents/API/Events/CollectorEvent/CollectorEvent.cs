using System;
using System.Collections.Generic;
using System.Linq;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using MEC;
using RandomEvents.API.Interfaces;
using UnityEngine;
using Random = UnityEngine.Random;
using Server = Exiled.Events.Handlers.Server;

namespace RandomEvents.API.Events.CollectorEvent;

public class CollectorEvent : Event
{
    public override string Name { get; } = "CollectorEvent";
    public override string DisplayName { get; } = "수집가";
    public override string Description { get; } = "랜덤한 SCP 아이템 1~3개를 들고 시작합니다.";

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

    public override void Run()
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

    public override void RegisterEvents()
    {
        Exiled.Events.Handlers.Player.ChangingRole += OnRoleChanging;
        Server.RespawningTeam += OnRespawningTeam;
    }

    public override void UnregisterEvents()
    {
        Exiled.Events.Handlers.Player.ChangingRole -= OnRoleChanging;
        Server.RespawningTeam -= OnRespawningTeam;
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

    private void OnRespawningTeam(RespawningTeamEventArgs ev)
    {
        Timing.CallDelayed(.1f, () =>
        {
            foreach (var player in ev.Players)
            {
                if (player.IsScp || !player.IsAlive) continue;

                for (var i = 0; i < Random.Range(1, 4); i++)
                {
                    player.AddItem(GetRandomScpItem());
                }
            }
        });
    }
}