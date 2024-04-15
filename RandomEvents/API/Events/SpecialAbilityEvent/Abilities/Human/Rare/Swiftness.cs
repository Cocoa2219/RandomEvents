using Exiled.API.Enums;
using Exiled.API.Features;
using RandomEvents.API.Events.SpecialAbilityEvent.Enums;

namespace RandomEvents.API.Events.SpecialAbilityEvent.Abilities.Human.Rare;

public class Swiftness : IAbility
{
    public void RegisterEvents()
    {
        Player.SyncEffect(new Effect(EffectType.MovementBoost, 0, 15));
    }

    public void UnregisterEvents()
    {
        Player.DisableEffect(EffectType.MovementBoost);
    }

    public AbilityType Type { get; } = AbilityType.RARE_SWIFTNESS;
    public Player Player { get; set; }
    public AbilityRole Role { get; } = AbilityRole.Human;
    public SpecialAbilityEvent.Rarity Rarity { get; } = SpecialAbilityEvent.Rarity.Rare;
    public string DisplayName { get; } = "신속 <color=#529CCA>(레어)</color>";
    public string Description { get; } = "이동 속도가 15% 증가합니다.";
    public SpecialAbilityEvent Event { get; set; }

    public object Clone()
    {
        return new Swiftness();
    }
}