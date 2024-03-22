using System.Linq;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;

namespace RandomEvents.API.Events.SMBEvent;

public class SmbEventHandler(SmbEvent smbEvent)
{
    private SmbEvent SmbEvent { get; set; } = smbEvent;

    private Player ChosenPlayer { get; set; }

    public void OnRoundStart()
    {
        SmbEvent.LogDebug("라운드가 시작되었습니다.");

        var playerList = Player.List.ToList();

        playerList.ShuffleList();

        ChosenPlayer = playerList[0];

        SmbEvent.LogInfo($"{ChosenPlayer.CustomName}님이 선택되었습니다.");
    }

    public void OnPlayerDying(DyingEventArgs ev)
    {
        if (ev.Player != ChosenPlayer)
            return;

        foreach (var player in Player.List)
        {
            if (player == ChosenPlayer)
                continue;

            player.Kill($"{ev.Player.CustomName}의 사망, 게임 종료!");
        }
    }
}