using Exiled.API.Features;
using RandomEvents.API.Events.SpecialAbilityEvent.Enums;

namespace RandomEvents.API.Events.SpecialAbilityEvent.Abilities.Human.Rare;

public class Resistance : IAbility
{
    public object Clone()
    {
        return new Resistance();
    }

    public void RegisterEvents()
    {
        Event.SetPlayerStats(Player, new PlayerStatus(0, 0.3f, 0));
    }

    public void UnregisterEvents()
    {
        Event.SetPlayerStats(Player, new PlayerStatus(0, 0, 0));
    }

    public AbilityType Type { get; } = AbilityType.RARE_RESISTANCE;
    public Player Player { get; set; }
    public AbilityRole Role { get; } = AbilityRole.Human;
    public SpecialAbilityEvent.Rarity Rarity { get; } = SpecialAbilityEvent.Rarity.Rare;
    public string DisplayName { get; } = "저항 <color=#529CCA>(레어)</color>";
    public string Description { get; } = "모든 피해량이 30% 감소합니다.";
    public SpecialAbilityEvent Event { get; set; }
}