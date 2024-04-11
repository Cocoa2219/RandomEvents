using System;
using System.Diagnostics.CodeAnalysis;
using CommandSystem;
using Exiled.API.Features;

namespace RandomEvents.Commands;

// [CommandHandler(typeof(ClientCommandHandler))]
public class Reroll // : ICommand
{
    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        var player = Player.Get(sender as CommandSender);

        if (player == null)
        {
            response = "플레이어를 찾을 수 없습니다.";
            return false;
        }

        if (RandomEvents.Instance.coreEventHandler.isRerolled)
        {
            response = "이미 재추첨을 진행했습니다.";
            return false;
        }

        if (RandomEvents.Instance.coreEventHandler.isRerolling)
        {
            var success = RandomEvents.Instance.coreEventHandler.RerollVote(player);

            response = success ? "재추첨 투표에 찬성했습니다." : "재추첨 투표에 반대했습니다.";
            return true;
        }

        if (RandomEvents.Instance.coreEventHandler.isEventRunning || Round.IsStarted)
        {
            response = "라운드가 시작되었습니다.";
            return false;
        }

        RandomEvents.Instance.coreEventHandler.StartRerollVote(player);

        response = "재추첨 투표를 시작했습니다.";
        return true;
    }

    public string Command { get; } = "reroll";
    public string[] Aliases { get; } = new string[] { "rr" };
    public string Description { get; } = "현재 이벤트를 재추첨합니다.";
}