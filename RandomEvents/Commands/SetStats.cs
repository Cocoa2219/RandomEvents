using System;
using System.Diagnostics.CodeAnalysis;
using CommandSystem;
using Exiled.API.Features;
using RandomEvents.API;

namespace RandomEvents.Commands;

[CommandHandler(typeof(ClientCommandHandler))]
public class SetStats : ICommand
{
    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, [UnscopedRef] out string response)
    {
        if (arguments.Count < 4)
        {
            response = "사용법 : .setstats <플레이어 ID / 닉네임> <ATK> <DEF> <Miss>";
            return false;
        }

        if (!float.TryParse(arguments.At(1), out var atk) || !float.TryParse(arguments.At(2), out var def) || !float.TryParse(arguments.At(3), out var miss))
        {
            response = "수만 입력해주세요.";
            return false;
        }

        if (atk < 0 || def < 0 || miss < 0)
        {
            response = "0 이상의 수만 입력해주세요.";
            return false;
        }

        var player = Player.Get(arguments.At(0));

        if (player == null)
        {
            response = "플레이어를 찾을 수 없습니다.";
            return false;
        }

        // RandomEvents.Instance.coreEventHandler.PlayerStatuses[player] = new PlayerStatus(atk, def, miss);
        response = $"{player.CustomName}의 스탯이 ATK : {atk} / DEF : {def} / Miss : {miss}로 설정되었습니다.";
        return true;
    }

    public string Command { get; } = "setstats";
    public string[] Aliases { get; } = ["ss"];
    public string Description { get; } = "플레이어의 스탯을 설정합니다.";
}