using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using RandomEvents.API.Events.SpecialAbilityEvent.Enums;

namespace RandomEvents.API.Events.SpecialAbilityEvent.Abilities.Human.Epic;

public class Absorption : IAbility
{
    public object Clone()
    {
        return new Absorption();
    }

    public void RegisterEvents()
    {
        Exiled.Events.Handlers.Player.Hurting += OnHurting;
    }

    public void UnregisterEvents()
    {
        Exiled.Events.Handlers.Player.Hurting -= OnHurting;
    }

    private void OnHurting(HurtingEventArgs ev)
    {
        if (ev.Attacker == null || ev.Attacker != Player)
            return;

        Player.AddAhp(ev.Amount * 0.2f);
    }

    public AbilityType Type { get; } = AbilityType.EPIC_ABSORPTION;
    public Player Player { get; set; }
    public AbilityRole Role { get; } = AbilityRole.Human;
    public SpecialAbilityEvent.Rarity Rarity { get; } = SpecialAbilityEvent.Rarity.Epic;
    public string DisplayName { get; } = "흡수 <color=#9A6DD7>(에픽)</color>";
    public string Description { get; } = "피해를 입힐 때마다 AHP을 회복합니다.";
    public SpecialAbilityEvent Event { get; set; }
}