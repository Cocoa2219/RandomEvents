using System.Linq;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;

namespace RandomEvents.API.Events.SMBEvent;

public class SmbEventHandler(SmbEvent smbEvent)
{
    private SmbEvent SmbEvent { get; set; } = smbEvent;

    public void OnPlayerDying(DyingEventArgs ev)
    {
        if (ev.Player != SmbEvent.ChosenPlayer)
            return;

        foreach (var player in Player.List)
        {
            if (player == SmbEvent.ChosenPlayer)
                continue;

            player.Kill($"{ev.Player.CustomName}의 사망, 게임 종료!");
        }
    }
}