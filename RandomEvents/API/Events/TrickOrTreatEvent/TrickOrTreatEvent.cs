using System;
using System.Collections.Generic;
using Exiled.API.Features.Pickups;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using Exiled.Events.Handlers;
using InventorySystem.Items.Usables.Scp330;
using MEC;
using UnityEngine;
using Random = UnityEngine.Random;
using Scp330Pickup = Exiled.API.Features.Pickups.Scp330Pickup;

namespace RandomEvents.API.Events.TrickOrTreatEvent;

public class TrickOrTreatEvent : Event
{
    public override string Name { get; } = "TrickOrTreatEvent";
    public override string DisplayName { get; } = "트릭 오어 트릿";
    public override string Description { get; } = "맛있는 사탕 좀 먹으면서 가자구요.";

    public override void Run()
    {
        Timing.CallDelayed(0.1f, () =>
        {
            foreach (var player in Exiled.API.Features.Player.List)
            {
                if (!player.IsAlive) continue;

                Timing.RunCoroutine(GetSomeCandies(player, 4));
            }
        });
    }

    private IEnumerator<float> GetSomeCandies(Exiled.API.Features.Player player, int count)
    {
        for (var i = 0; i < count; i++)
        {
            player.TryAddCandy(GetRandomCandy());
            yield return Timing.WaitForSeconds(0.1f);
        }
    }

    public override void RegisterEvents()
    {
        Player.ChangingRole += OnChangingRole;
        Player.Dying += OnDying;
        Server.RespawningTeam += OnRespawningTeam;
    }

    public override void UnregisterEvents()
    {
        Player.ChangingRole -= OnChangingRole;
        Player.Dying -= OnDying;
        Server.RespawningTeam -= OnRespawningTeam;
    }

    private void OnRespawningTeam(RespawningTeamEventArgs ev)
    {
        Timing.CallDelayed(.1f, () =>
        {
            foreach (var player in ev.Players)
            {
                Timing.RunCoroutine(GetSomeCandies(player, 4));
            }
        });
    }

    public void OnChangingRole(ChangingRoleEventArgs ev)
    {
        Timing.CallDelayed(.1f, () =>
        {
            Timing.RunCoroutine(GetSomeCandies(ev.Player, 4));
        });
    }

    private CandyKindID GetRandomCandy()
    {
        // Return a random CandyKindID except for CandyKindID.None
        return (CandyKindID)Random.Range(1, Enum.GetValues(typeof(CandyKindID)).Length);
    }

    private void OnDying(DyingEventArgs ev)
    {
        var pickup = Pickup.CreateAndSpawn(ItemType.SCP330, ev.Player.Position + new Vector3(0f,0.21f,0f), Quaternion.identity);

        pickup.As<Scp330Pickup>().Candies = [GetRandomCandy()];
        pickup.As<Scp330Pickup>().ExposedCandy = pickup.As<Scp330Pickup>().Candies[0];
    }
}