using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using CommandSystem;
using Exiled.API.Features;

namespace RandomEvents.API.Events.SpecialAbilityEvent.Commands;

[CommandHandler(typeof(ClientCommandHandler))]
public class ForceAbility : ICommand
{
    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, [UnscopedRef] out string response)
    {
        if (arguments.Count < 1)
        {
            response = "사용법: forceability <list/give>";
            return false;
        }

        var @event = RandomEvents.Instance.CoreEventHandler.CurrentEvent;

        if (@event is null || @event.Name != "SpecialAbilityEvent")
        {
            response = "이벤트가 활성화되어 있지 않습니다.";
            return false;
        }

        var speicalAbilityEvent = (SpecialAbilityEvent)@event;

        switch (arguments.At(0))
        {
            case "list":
                var sb = new StringBuilder();
                sb.AppendLine("<color=#ffffff>능력 목록:</color>");
                foreach (var ability in speicalAbilityEvent.AbilitiesList)
                {
                    sb.AppendLine($"<color=#ffffff> - {ability.DisplayName} {speicalAbilityEvent.GetRarityString(ability.Rarity)} ({ability.Type.ToString()})</color>");
                    sb.AppendLine($"<color=#ffffff>   {ability.Description}</color>");
                }

                response = sb.ToString();
                return true;
            case "give":
                if (arguments.Count < 3)
                {
                    response = "사용법: forceability give <플레이어 이름> <능력 이름>";
                    return false;
                }

                var player = Player.Get(arguments.At(1));

                if (player == null)
                {
                    response = "플레이어를 찾을 수 없습니다.";
                    return false;
                }

                var abilityName = string.Join(" ", arguments.Skip(2));

                var ab = speicalAbilityEvent.AbilitiesList.FirstOrDefault(x => string.Equals(x.Type.ToString(), abilityName, StringComparison.CurrentCultureIgnoreCase));

                if (ab == null)
                {
                    response = "능력을 찾을 수 없습니다.";
                    return false;
                }

                speicalAbilityEvent.GiveAbility(player, ab.Type);
                response = "능력을 부여했습니다.";
                return true;
            default:
                response = "사용법: forceability <list/give>";
                return false;
        }
    }

    public string Command { get; } = "forceability";
    public string[] Aliases { get; } = { "fa" };
    public string Description { get; } = "플레이어에게 특수 능력을 부여합니다.";
}