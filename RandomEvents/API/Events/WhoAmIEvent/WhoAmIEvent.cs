using System;
using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using MEC;
using PlayerRoles;
using RandomEvents.API.Interfaces;
using Random = UnityEngine.Random;

namespace RandomEvents.API.WhoAmIEvent;

public class WhoAmIEvent : IEvent
{
    public string Name { get; } = "WhoAmIEvent";
    public string DisplayName { get; } = "나는 누구?";
    public string Description { get; } = "1분마다 진영이 바뀝니다.";

    private RoleTypeId GetRandomRole()
    {
        var roleTypes = Enum.GetValues(typeof(RoleTypeId));
        RoleTypeId randomRole;
        do
        {
            randomRole = (RoleTypeId) roleTypes.GetValue(Random.Range(0, roleTypes.Length));
        } while (randomRole is RoleTypeId.None or RoleTypeId.Scp079);

        return randomRole;
    }

    private IEnumerator<float> WhoAmICoroutine()
    {
        while (!Round.IsEnded)
        {
            yield return Timing.WaitForSeconds(60f);

            foreach (var player in Player.List)
            {
                if (!player.IsAlive) continue;

                var currentItems = player.CurrentItem;

                player.Role.Set(GetRandomRole(), SpawnReason.Respawn, RoleSpawnFlags.None);

                player.CurrentItem = currentItems;
            }
        }
    }

    public void Run()
    {
        Timing.RunCoroutine(WhoAmICoroutine());
    }

    public void RegisterEvents()
    {

    }

    public void UnregisterEvents()
    {

    }
}