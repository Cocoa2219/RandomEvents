using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Roles;
using MEC;
using PlayerRoles;
using RandomEvents.API.Interfaces;
using UnityEngine;

namespace RandomEvents.API.Events.SCPRushEvent;

public class ScpRushEvent : IEvent
{
    public string Name { get; } = "ScpRushEvent";
    public string DisplayName { get; } = "SCP 러쉬";
    public string Description { get; } = "모든 SCP가 한 종류로 통일됩니다.";

    private RoleTypeId GetRandomSCP()
    {
        var scps = new[]
        {
            RoleTypeId.Scp049,
            RoleTypeId.Scp079,
            RoleTypeId.Scp096,
            RoleTypeId.Scp106,
            RoleTypeId.Scp173,
            RoleTypeId.Scp939,
        };

        return scps[Random.Range(0, scps.Length)];
    }

    public void Run()
    {
        var role = GetRandomSCP();
        Timing.CallDelayed(0.1f, () =>
        {
            foreach (var scp in Player.Get(Side.Scp))
            {
                scp.Role.Set(role, SpawnReason.Respawn, RoleSpawnFlags.All);

                if (role == RoleTypeId.Scp079)
                {
                    scp.Role.As<Scp079Role>().Level = 5;
                }
            }
        });
    }

    public void RegisterEvents()
    {

    }

    public void UnregisterEvents()
    {

    }
}