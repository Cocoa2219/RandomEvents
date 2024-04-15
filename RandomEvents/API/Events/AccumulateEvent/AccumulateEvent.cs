using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using Exiled.Events.Handlers;
using MEC;

namespace RandomEvents.API.Events.AccumulateEvent;

public class AccumulateEvent : Event
{
    public override string Name { get; } = "AccumulateEvent";
    public override string DisplayName { get; } = "중첩";
    public override string Description { get; } = "HP의 제한이 없어집니다.";
    public override void Run()
    {
        foreach (var player in Exiled.API.Features.Player.List)
        {
            player.MaxHealth = float.MaxValue;
        }
    }

    public override void RegisterEvents()
    {
        Player.ChangingRole += OnChangingRole;
        Server.RespawningTeam += OnRespawningTeam;
    }

    public override void UnregisterEvents()
    {
        Player.ChangingRole -= OnChangingRole;
        Server.RespawningTeam -= OnRespawningTeam;
    }

    public void OnChangingRole(ChangingRoleEventArgs ev)
    {
        Timing.CallDelayed(.1f, () =>
        {
            ev.Player.MaxHealth = float.MaxValue;
        });
    }

    private void OnRespawningTeam(RespawningTeamEventArgs ev)
    {
        Timing.CallDelayed(.1f, () =>
        {
            foreach (var player in ev.Players)
            {
                player.MaxHealth = float.MaxValue;
            }
        });
    }
}