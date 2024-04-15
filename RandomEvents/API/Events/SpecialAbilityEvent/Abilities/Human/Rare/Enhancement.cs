using Exiled.API.Features;
using RandomEvents.API.Events.SpecialAbilityEvent.Enums;

namespace RandomEvents.API.Events.SpecialAbilityEvent.Abilities.Human.Rare;

public class Enhancement : IAbility
{
    public object Clone()
    {
        return new Enhancement();
    }

    public void RegisterEvents()
    {
        Event.SetPlayerStats(Player, new PlayerStatus(0.2f, 0, 0));
    }

    public void UnregisterEvents()
    {
        Event.SetPlayerStats(Player, new PlayerStatus(0, 0, 0));
    }

    public AbilityType Type { get; } = AbilityType.RARE_ENHANCEMENT;
    public Player Player { get; set; }
    public AbilityRole Role { get; } = AbilityRole.Human;
    public SpecialAbilityEvent.Rarity Rarity { get; } = SpecialAbilityEvent.Rarity.Rare;
    public string DisplayName { get; } = "강화";
    public string Description { get; } = "모든 데미지가 20% 증가합니다.";
    public SpecialAbilityEvent Event { get; set; }
}