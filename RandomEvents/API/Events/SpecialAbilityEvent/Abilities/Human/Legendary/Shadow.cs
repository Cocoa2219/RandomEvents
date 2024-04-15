using Exiled.API.Features;
using RandomEvents.API.Events.SpecialAbilityEvent.Enums;

namespace RandomEvents.API.Events.SpecialAbilityEvent.Abilities.Human.Legendary;

public class Shadow : IAbility
{
    public object Clone()
    {
        return new Shadow();
    }

    // TODO: Implement Shadow Ability (Important)

    public void RegisterEvents()
    {
    }

    public void UnregisterEvents()
    {

    }

    public AbilityType Type { get; } = AbilityType.LEGENDARY_SHADOW;
    public Player Player { get; set; }
    public AbilityRole Role { get; } = AbilityRole.Human;
    public SpecialAbilityEvent.Rarity Rarity { get; } = SpecialAbilityEvent.Rarity.Legendary;
    public string DisplayName { get; } = "그림자";
    public string Description { get; } = "은신입니다.";
    public SpecialAbilityEvent Event { get; set; }
}