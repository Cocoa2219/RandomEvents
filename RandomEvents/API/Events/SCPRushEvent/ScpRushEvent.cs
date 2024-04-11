using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Roles;
using MEC;
using PlayerRoles;
using RandomEvents.API.Interfaces;
using UnityEngine;

namespace RandomEvents.API.Events.SCPRushEvent;

public class ScpRushEvent : Event
{
    public override string Name { get; } = "ScpRushEvent";
    public override string DisplayName { get; } = "SCP 러쉬";
    public override string Description { get; } = "모든 SCP가 한 종류로 통일됩니다.";

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

    public override void Run()
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

    public override void RegisterEvents()
    {

    }

    public override void UnregisterEvents()
    {

    }
}