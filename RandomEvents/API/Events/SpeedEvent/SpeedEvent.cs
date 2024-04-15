using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using MEC;
using Player = Exiled.Events.Handlers.Player;

namespace RandomEvents.API.Events.SpeedEvent;

public class SpeedEvent : Event
{
    public override string Name { get; } = "SpeedEvent";
    public override string DisplayName { get; } = "한국인이 좋아하는 속도";
    public override string Description { get; } = "상당한 속도로 게임을 즐겨보세요.";
    public override void Run()
    {
        Timing.CallDelayed(0.1f, () =>
        {
            foreach (var player in Exiled.API.Features.Player.List)
            {
                if (!player.IsAlive) continue;

                player.SyncEffects([new Effect(EffectType.MovementBoost, 0, 75), new Effect(EffectType.Scp1853, 0, 5)]);
            }
        });
    }

    public override void RegisterEvents()
    {
        Player.ChangingRole += OnChangingRole;
        Exiled.Events.Handlers.Server.RespawningTeam += OnRespawningTeam;
        Player.SearchingPickup += OnSearchingPickup;
    }

    public override void UnregisterEvents()
    {
        Exiled.Events.Handlers.Player.ChangingRole -= OnChangingRole;
        Exiled.Events.Handlers.Server.RespawningTeam -= OnRespawningTeam;
        Player.SearchingPickup -= OnSearchingPickup;
    }

    private void OnSearchingPickup(SearchingPickupEventArgs ev)
    {
        ev.SearchTime = 0f;
    }

    public void OnChangingRole(ChangingRoleEventArgs ev)
    {
        Timing.CallDelayed(.1f, () =>
        {
            ev.Player.SyncEffects([new Effect(EffectType.MovementBoost, 0, 75), new Effect(EffectType.Scp1853, 0, 5)]);
        });
    }

    private void OnRespawningTeam(RespawningTeamEventArgs ev)
    {
        Timing.CallDelayed(.1f, () =>
        {
            foreach (var player in ev.Players)
            {
                player.SyncEffects([new Effect(EffectType.MovementBoost, 0, 75), new Effect(EffectType.Scp1853, 0, 5)]);
            }
        });
    }
}