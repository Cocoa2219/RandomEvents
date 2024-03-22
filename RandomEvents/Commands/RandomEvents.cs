using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CommandSystem;
using Exiled.API.Features;
using UnityEngine.ProBuilder.MeshOperations;

namespace RandomEvents.Commands;

[CommandHandler(typeof(ClientCommandHandler))]
public class RandomEventsCommand : ICommand
{
    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, [UnscopedRef] out string response)
    {
        var player = Player.Get((CommandSender)sender);

        if (player is null)
        {
            response = "이 명령어는 플레이어만 실행 할 수 있습니다.";
            return false;
        }

        if (arguments.Count < 1)
        {
            response = "사용법 : randomevents about/list/run";
            return false;
        }

        var subcommand = arguments.At(0);

        switch (subcommand)
        {
            case "about":
                response = "Made by Cocoa (@cocoa_1.19)";
                return true;
            case "list":
                var text = RandomEvents.Instance.coreEventHandler.Events.Aggregate("이벤트 목록 : \n",
                    (current, @event) => current + $"{@event.DisplayName} ({@event.Name}) - {@event.Description}\n");
                response = text;
                return true;
            case "run":
                if (arguments.Count < 2)
                {
                    response = "사용법 : randomevents run <이벤트 이름>";
                    return false;
                }

                var selEvent =
                    RandomEvents.Instance.coreEventHandler.Events.FirstOrDefault(x => x.Name.ToLower() == arguments.At(1).ToLower());

                if (selEvent is null)
                {
                    response = $"{arguments.At(1)} 이벤트가 없습니다.";
                    return false;
                }

                if (RandomEvents.Instance.coreEventHandler.RunEvent(selEvent))
                {
                    response = $"{arguments.At(1)} 이벤트가 실행되었습니다.";
                    return true;
                }

                response = $"{arguments.At(1)} 이벤트 실행 도중 오류가 발생했습니다.";
                return false;
            default:
                response = "사용법 : randomevents about/list/run";
                return false;
        }
    }

    public string Command { get; } = "randomevents";
    public string[] Aliases { get; } = [ "re" ];
    public string Description { get; } = "랜덤 이벤트 플러그인입니다.";
}