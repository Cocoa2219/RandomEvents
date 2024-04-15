using Exiled.API.Enums;
using Exiled.API.Features;
using RandomEvents.API.Events.SpecialAbilityEvent.Enums;

namespace RandomEvents.API.Events.SpecialAbilityEvent.Abilities.Scp096.Special;

public class Impenetrable : IAbility
{
    public object Clone()
    {
        return new Impenetrable();
    }

    public void RegisterEvents()
    {
        Event.AddPlayerStats(Player, new PlayerStatus(3f, 0.25f, 0f));
        Player.SyncEffect(new Effect(EffectType.MovementBoost, 0, 10));
    }

    public void UnregisterEvents()
    {
        Event.AddPlayerStats(Player, new PlayerStatus(-3f, -0.25f, 0f));
        Player.DisableEffect(EffectType.MovementBoost);
    }

    public AbilityType Type { get; } = AbilityType.SCP_096_IMPENETRABLE;
    public Player Player { get; set; }
    public AbilityRole Role { get; } = AbilityRole.Scp096;
    public SpecialAbilityEvent.Rarity Rarity { get; } = SpecialAbilityEvent.Rarity.Special;
    public string DisplayName { get; } = "난공불락";
    public string Description { get; } = "공격력이 300% 증가하고, 이동 속도가 10% 증가하고, 방어력이 25% 증가합니다.";
    public SpecialAbilityEvent Event { get; set; }
}