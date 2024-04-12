using System.Collections.Generic;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using Player = Exiled.Events.Handlers.Player;

namespace RandomEvents.API.Events.YCTEEvent;

public class YCTEEvent : Event
{
    public override string Name { get; } = "YCTEEvent";
    public override string DisplayName { get; } = "어제의 동지는 오늘의 적";
    public override string Description { get; } = "아군 공격이 허용됩니다. - 현재 공사 중...";

    private Dictionary<Exiled.API.Features.Player, int> FFCount = new();
    private Dictionary<Exiled.API.Features.Player, int> BadFFCount = new();

    public override void Run()
    {
        Server.FriendlyFire = true;
    }

    public override void RegisterEvents()
    {
        Player.Dying += OnDying;
        Player.ChangingRole += OnChangingRole;
    }

    public override void UnregisterEvents()
    {
        Player.Dying -= OnDying;
        Player.ChangingRole -= OnChangingRole;
    }

    private void OnDying(DyingEventArgs ev)
    {
        if (ev.Attacker == null || ev.Player == null) return;

        if (ev.Attacker.Role.Team != ev.Player.Role.Team) return;
        if (!FFCount.TryAdd(ev.Attacker, 1))
        {
            FFCount[ev.Attacker]++;
            if (FFCount[ev.Attacker] >= 4)
            {
                BadFFCount.TryAdd(ev.Attacker, 0);

                BadFFCount[ev.Attacker] = FFCount[ev.Attacker] - 3;

                var atk = BadFFCount[ev.Attacker] / 4 > 1 ? 1 : BadFFCount[ev.Attacker] / 4;
                var def = BadFFCount[ev.Attacker] / 4 > 1 ? 1 : BadFFCount[ev.Attacker] / 4;

                // RandomEvents.Instance.coreEventHandler.SetStats(ev.Attacker, new PlayerStatus(atk, -def, 0));
            }
        }
    }

    private void OnChangingRole(ChangingRoleEventArgs ev)
    {
        if (BadFFCount.ContainsKey(ev.Player))
        {
            BadFFCount.Remove(ev.Player);
        }

        if (FFCount.ContainsKey(ev.Player))
        {
            FFCount.Remove(ev.Player);
        }

        // RandomEvents.Instance.coreEventHandler.SetStats(ev.Player, new PlayerStatus(0, 0, 0));
    }
}