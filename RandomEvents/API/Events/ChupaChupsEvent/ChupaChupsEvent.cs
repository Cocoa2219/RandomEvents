using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using Exiled.Events.Handlers;
using MEC;

namespace RandomEvents.API.Events.ChupaChupsEvent;

public class ChupaChupsEvent : Event
{
    public override string Name { get; } = "ChupaChupsEvent";
    public override string DisplayName { get; } = "츄파춥스";
    public override string Description { get; } = "맛있는 사탕 한 자루.";
    public override void Run()
    {
        Timing.CallDelayed(0.1f, () =>
        {
            foreach (var player in Exiled.API.Features.Player.List)
            {
                if (!player.IsAlive) continue;

                player.AddItem(ItemType.Jailbird);
            }
        });
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
            ev.Player.AddItem(ItemType.Jailbird);
        });
    }

    private void OnRespawningTeam(RespawningTeamEventArgs ev)
    {
        Timing.CallDelayed(.1f, () =>
        {
            foreach (var player in ev.Players)
            {
                player.AddItem(ItemType.Jailbird);
            }
        });
    }
}