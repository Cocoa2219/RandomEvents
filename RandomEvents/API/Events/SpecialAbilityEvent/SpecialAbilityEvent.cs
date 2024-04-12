using System;
using System.Collections.Generic;
using System.Linq;
using Exiled.API.Features;
using RandomEvents.API.Events.SpecialAbilityEvent.Enums;

namespace RandomEvents.API.Events.SpecialAbilityEvent;

public class SpecialAbilityEvent : Event
{
    public override string Name { get; } = "SpecialAbilityEvent";
    public override string DisplayName { get; } = "특수 능력";
    public override string Description { get; } = "자신을 더욱 강력하게 만들어줄 능력을 부여받습니다. - 매우, 심각하게, 상당한 시간이 걸릴 것으로 예상됩니다.";

    public Dictionary<Player, Abilities> PlayerAbilitiesMap { get; set; } = new();

    private List<Abilities> HumanAbilities { get; } = Enum.GetValues(typeof(Abilities)).Cast<Abilities>().Where(ability => !ability.ToString().StartsWith("SCP")).ToList();

    public override void Run()
    {

    }

    public override void RegisterEvents()
    {

    }

    public override void UnregisterEvents()
    {

    }

    // private bool GiveAbility(Player player)
    // {
    //     if (PlayerAbilitiesMap.ContainsKey(player))
    //     {
    //         PlayerAbilitiesMap.Remove(player);
    //     }
    //
    //     if (!player.IsAlive) return false;
    //
    //     switch (player.IsHuman)
    //     {
    //         case true:
    //             // From Abilities Enum, select a random element which not starts with "SCP"
    //
    //         case false:
    //             var scpAbility = Abilities.SCP_049_INFECTIOUS_EQUIPMENT;
    //             PlayerAbilitiesMap.Add(player, scpAbility);
    //             player.Broadcast(5, $"당신은 {scpAbility} 능력을 부여받았습니다.");
    //             break;
    //     }
    // }
}