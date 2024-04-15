using Exiled.API.Enums;
using Exiled.API.Features;
using RandomEvents.API.Events.SpecialAbilityEvent.Enums;

namespace RandomEvents.API.Events.SpecialAbilityEvent.Abilities.Scp0492.Unique;

public class Awakening : IAbility
{
    public object Clone()
    {
        return new Awakening();
    }

    public void RegisterEvents()
    {
        Event.AddPlayerStats(Player, new PlayerStatus(50, 0, 0));
        Player.SyncEffect(new Effect(EffectType.MovementBoost, 0, 10));
    }

    public void UnregisterEvents()
    {
        Event.AddPlayerStats(Player, new PlayerStatus(-50, 0, 0));
        Player.DisableEffect(EffectType.MovementBoost);
    }

    public AbilityType Type { get; } = AbilityType.SCP_049_2_AWAKENING;
    public Player Player { get; set; }
    public AbilityRole Role { get; } = AbilityRole.Scp0492;
    public SpecialAbilityEvent.Rarity Rarity { get; } = SpecialAbilityEvent.Rarity.Unique;
    public string DisplayName { get; } = "각성";
    public string Description { get; } = "공격력이 50% 증가하고, 이동 속도가 10% 증가합니다.";
    public SpecialAbilityEvent Event { get; set; }
}