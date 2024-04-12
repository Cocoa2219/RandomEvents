using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using MEC;
using UnityEngine;
using Player = Exiled.Events.Handlers.Player;

namespace RandomEvents.API.Events.BlackoutEvent;

public class BlackoutEvent : Event
{
    public override string Name { get; } = "BlackoutEvent";
    public override string DisplayName { get; } = "블랙 아웃";
    public override string Description { get; } = "시설의 곳곳이 정전됩니다.";
    public override void Run()
    {
        foreach (var room in Room.List)
        {
            room.TurnOffLights();
        }

        var emergencyRooms = new Dictionary<ZoneType, int>
        {
            { ZoneType.Entrance, Random.Range(2, 3) },
            { ZoneType.LightContainment, Random.Range(2, 3) },
            { ZoneType.HeavyContainment, Random.Range(2, 3) },
            { ZoneType.Surface, 0 }
        };

        foreach (var room in emergencyRooms)
        {
            // Pick Random Room
            for (var i = 0; i < room.Value; i++)
            {
                var rm = Room.Get(room.Key).GetRandomValue();
                rm.TurnOffLights(0f);
                rm.RoomLightController.NetworkOverrideColor = new Color32(255, 0, 0, 30);
            }
        }

        Timing.CallDelayed(.1f, () =>
        {
            foreach (var player in Exiled.API.Features.Player.List)
            {
                if (!player.IsAlive) continue;

                player.AddItem(ItemType.Flashlight);
            }

        });
    }

    public override void RegisterEvents()
    {
        Player.ChangingRole += OnChangingRole;
    }

    public override void UnregisterEvents()
    {
        Player.ChangingRole -= OnChangingRole;
    }

    private void OnChangingRole(ChangingRoleEventArgs ev)
    {
        Timing.CallDelayed(.1f, () =>
        {
            ev.Player.AddItem(ItemType.Flashlight);
        });
    }
}