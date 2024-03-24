using System;
using System.Diagnostics.CodeAnalysis;
using CommandSystem;
using Exiled.API.Features;

namespace RandomEvents.Commands;

public class Reroll // : ICommand
{
    public void Execute(ArraySegment<string> arguments, ICommandSender sender)
    {
        if (RandomEvents.Instance.coreEventHandler.isRerolling)
        {

        }
        else
        {
            if (RandomEvents.Instance.coreEventHandler.isEventRunning || Round.IsStarted)
            {
                return;
            }

            RandomEvents.Instance.coreEventHandler.isRerolling = true;
            RandomEvents.Instance.coreEventHandler.rerollPlayers = Player.List.Count / 2;

            Round.IsLobbyLocked = true;

            return;
        }
    }

    public string Command { get; } = "reroll";
    public string[] Aliases { get; } = new string[] { "rr" };
    public string Description { get; } = "현재 이벤트를 재추첨합니다.";
}