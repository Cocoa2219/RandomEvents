using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using RandomEvents.API.Events.SpecialAbilityEvent.Enums;

namespace RandomEvents.API.Events.SpecialAbilityEvent.Abilities.Human.Special;

public class GoldenBell : IAbility
{
    public object Clone()
    {
        return new GoldenBell();
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
        if (ev.Player != Player) return;

        var diff = 25 - ev.Amount;

        if (!(diff > 0)) return;
        ev.Amount = 25;
        ev.Player.AddAhp(diff, 1000f);
    }

    public AbilityType Type { get; } = AbilityType.SPECIAL_GOLDEN_BELL;
    public Player Player { get; set; }
    public AbilityRole Role { get; } = AbilityRole.Human;
    public SpecialAbilityEvent.Rarity Rarity { get; } = SpecialAbilityEvent.Rarity.Special;
    public string DisplayName { get; } = "금강불괴";
    public string Description { get; } = "받는 피해 최대치가 25로 고정됩니다.";
    public SpecialAbilityEvent Event { get; set; }
}