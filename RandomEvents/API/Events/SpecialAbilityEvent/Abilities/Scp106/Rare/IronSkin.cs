using Exiled.API.Features;
using RandomEvents.API.Events.SpecialAbilityEvent.Enums;

namespace RandomEvents.API.Events.SpecialAbilityEvent.Abilities.Scp106.Rare;

public class IronSkin : IAbility
{
    public object Clone()
    {
        return new IronSkin();
    }

    public void RegisterEvents()
    {
        Event.AddPlayerStats(Player, new PlayerStatus(0, 0.1f, 0f));
    }

    public void UnregisterEvents()
    {
        Event.AddPlayerStats(Player, new PlayerStatus(0, -0.1f, 0f));
    }

    public AbilityType Type { get; } = AbilityType.SCP_106_IRON_SKIN;
    public Player Player { get; set; }
    public AbilityRole Role { get; } = AbilityRole.Scp106;
    public SpecialAbilityEvent.Rarity Rarity { get; } = SpecialAbilityEvent.Rarity.Rare;
    public string DisplayName { get; } = "철갑";
    public string Description { get; } = "방어력이 10% 증가합니다.";
    public SpecialAbilityEvent Event { get; set; }
}