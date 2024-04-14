using System.Collections.Generic;
using System.Linq;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using MEC;

namespace RandomEvents.API.Events.BlessingEvent;

public class BlessingEvent : Event
{
    public override string Name { get; } = "BlessingEvent";
    public override string DisplayName { get; } = "축복";
    public override string Description { get; } = "관심을 받을 수록 강력해집니다.";

    private Dictionary<Player, HashSet<Player>> PlayerSpecs { get; set; } = new();
    private Dictionary<Player, PlayerStatus> PlayerStats { get; set; } = new();

    public override void Run()
    {
        Timing.RunCoroutine(SpectatorCoroutine());
    }

    public override void RegisterEvents()
    {
        Exiled.Events.Handlers.Player.Hurting += OnHurting;
    }

    public override void UnregisterEvents()
    {
        Exiled.Events.Handlers.Player.Hurting -= OnHurting;
    }

    private IEnumerator<float> SpectatorCoroutine()
    {
        while (true)
        {
            foreach (var player in Player.List)
            {
                if (!PlayerSpecs.ContainsKey(player))
                {
                    PlayerSpecs[player] = [];
                }

                if (!PlayerStats.ContainsKey(player))
                {
                    PlayerStats[player] = new PlayerStatus(0,0,0);
                }

                if (player.IsAlive)
                {
                    PlayerSpecs[player] = player.CurrentSpectatingPlayers.ToHashSet();
                }

                PlayerStats[player] = new PlayerStatus(PlayerSpecs[player].Count * 0.1f, PlayerSpecs[player].Count * 0.1f, 0);

                foreach (var spec in PlayerSpecs[player])
                {
                    var pl = PlayerSpecs.FirstOrDefault(x => x.Value.Contains(spec));

                    spec.ShowHint($"\n현재 {pl.Value.Count}명의 플레이어가 이 플레이어를 지켜보고 있습니다.", 1f);
                }
            }

            yield return Timing.WaitForSeconds(0.5f);
        }
    }

    private void OnHurting(HurtingEventArgs ev)
    {
        PlayerStats.TryAdd(ev.Player, new PlayerStatus(0, 0, 0));
        if (ev.Attacker == null)
        {
            ev.Amount = PlayerStatus.CalculateDamage(new PlayerStatus(0,0, 0), PlayerStats[ev.Player], ev.Amount);
            return;
        }

        PlayerStats.TryAdd(ev.Attacker, new PlayerStatus(0, 0, 0));

        ev.Amount = PlayerStatus.CalculateDamage(PlayerStats[ev.Attacker], PlayerStats[ev.Player], ev.Amount);
    }
}