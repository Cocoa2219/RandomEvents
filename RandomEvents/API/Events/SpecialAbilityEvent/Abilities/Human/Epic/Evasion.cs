using Exiled.API.Features;
using RandomEvents.API.Events.SpecialAbilityEvent.Enums;

namespace RandomEvents.API.Events.SpecialAbilityEvent.Abilities.Human.Epic;

public class Evasion : IAbility
{
    public object Clone()
    {
        return new Evasion();
    }

    public void RegisterEvents()
    {
        Event.SetPlayerStats(Player, new PlayerStatus(0, 0, 0.7f));
    }

    public void UnregisterEvents()
    {
        Event.SetPlayerStats(Player, new PlayerStatus(0, 0, 0));
    }

    public AbilityType Type { get; } = AbilityType.EPIC_EVASION;
    public Player Player { get; set; }
    public AbilityRole Role { get; } = AbilityRole.Human;
    public SpecialAbilityEvent.Rarity Rarity { get; } = SpecialAbilityEvent.Rarity.Epic;
    public string DisplayName { get; } = "회피";
    public string Description { get; } = "회피율 +70%";
    public SpecialAbilityEvent Event { get; set; }
}