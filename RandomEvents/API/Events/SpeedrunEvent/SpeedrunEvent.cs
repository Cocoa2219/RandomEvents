using System;
using System.Collections.Generic;
using System.Linq;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Pickups;
using Exiled.Events.EventArgs.Player;
using MEC;
using PlayerRoles;
using RandomEvents.API.Interfaces;

namespace RandomEvents.API.SpeedrunEvent;

public class SpeedrunEvent : IEvent
{
    public string Name { get; } = "SpeedrunEvent";
    public string DisplayName { get; } = "스피드런";
    public string Description { get; } = "가장 먼저 탈출하는 사람이 승리합니다.";
    public void Run()
    {
        Round.IsLocked = true;
        Timing.CallDelayed(0.1f, () =>
        {

            Pickup.List.ToList().ForEach(x => x.Destroy());

            foreach (var player in Player.List)
            {
                if (!player.IsAlive) continue;

                player.Role.Set(RoleTypeId.ClassD, SpawnReason.Respawn, RoleSpawnFlags.All);

                player.SyncEffect(new Effect(EffectType.MovementBoost, 0, 20, false, true));
                player.SyncEffect(new Effect(EffectType.Ensnared, 10, 1, false, true));

                player.Broadcast(5, "<size=30>스피드런 이벤트가 10초 뒤에 시작됩니다!</size>");
            }
        });

        Timing.CallDelayed(10f, () =>
        {
            Map.Broadcast(5, "<size=30>시작!</size>", Broadcast.BroadcastFlags.Normal, true);
            Timing.RunCoroutine(SpeedrunCoroutine());
        });
    }

    private IEnumerator<float> SpeedrunCoroutine()
    {
        while (!Round.IsEnded)
        {
            yield return Timing.WaitForSeconds(1f);

            foreach (var player in Player.List)
            {
                switch (player.CurrentRoom.Zone)
                {
                    case ZoneType.LightContainment:
                        player.DisableEffect(EffectType.MovementBoost);
                        player.SyncEffect(new Effect(EffectType.MovementBoost, 0, 20, false, true));
                        break;
                    case ZoneType.HeavyContainment:
                        player.DisableEffect(EffectType.MovementBoost);
                        player.SyncEffect(new Effect(EffectType.MovementBoost, 0, 30, false, true));
                        break;
                    case ZoneType.Entrance:
                        player.DisableEffect(EffectType.MovementBoost);
                        player.SyncEffect(new Effect(EffectType.MovementBoost, 0, 40, false, true));
                        break;
                    case ZoneType.Surface:
                        player.DisableEffect(EffectType.MovementBoost);
                        player.SyncEffect(new Effect(EffectType.MovementBoost, 0, 50, false, true));
                        break;
                }
            }
        }
    }

    public void RegisterEvents()
    {
        Exiled.Events.Handlers.Player.Escaping += OnEscaping;
    }

    public void UnregisterEvents()
    {
        Exiled.Events.Handlers.Player.Escaping -= OnEscaping;
    }

    private void OnEscaping(EscapingEventArgs ev)
    {
        foreach (var player in Player.List)
        {
            if (player.IsAlive)
            {
                player.Broadcast(5, $"<b>{ev.Player.Nickname}님이 스피드런 이벤트에서 승리했습니다!</b>");
            }

            player.Role.Set(RoleTypeId.Spectator, SpawnReason.ForceClass, RoleSpawnFlags.All);
        }

        Round.IsLocked = false;
    }
}