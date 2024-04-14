using System;
using System.Diagnostics.CodeAnalysis;
using CommandSystem;
using Exiled.API.Features;

namespace RandomEvents.Commands;

[CommandHandler(typeof(ClientCommandHandler))]
public class First : ICommand
{
    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, [UnscopedRef] out string response)
    {
        var player = Player.Get(sender);

        if (player == null)
        {
            response = "이 명령어는 플레이어만 사용할 수 있습니다.";
            return false;
        }

        RandomEvents.Instance.CoreEventHandler.Vote(0, player);
        response = "1번째 이벤트에 투표했습니다.";
        return true;
    }

    public string Command { get; } = "1";
    public string[] Aliases { get; } = ["first"];
    public string Description { get; } = "1번째 이벤트에 투표합니다.";
}