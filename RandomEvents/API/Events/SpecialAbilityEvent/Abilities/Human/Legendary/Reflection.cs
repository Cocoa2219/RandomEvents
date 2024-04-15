using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using RandomEvents.API.Events.SpecialAbilityEvent.Enums;
using UnityEngine;

namespace RandomEvents.API.Events.SpecialAbilityEvent.Abilities.Human.Legendary;

public class Reflection : IAbility
{
    public object Clone()
    {
        return new Reflection();
    }

    public void RegisterEvents()
    {
        Exiled.Events.Handlers.Player.Hurt += OnHurt;
    }

    public void UnregisterEvents()
    {
        Exiled.Events.Handlers.Player.Hurt -= OnHurt;
    }

    private void OnHurt(HurtEventArgs ev)
    {
        if (ev.Attacker == null) return;
        if (ev.Player != Player) return;

        var amount = Mathf.Max(ev.Amount * 0.4f, 100f);

        ev.Attacker.Hurt(ev.Player, amount, DamageType.Custom, null, null);
    }

    public AbilityType Type { get; } = AbilityType.LEGENDARY_REFLECTION;
    public Player Player { get; set; }
    public AbilityRole Role { get; } = AbilityRole.Human;
    public SpecialAbilityEvent.Rarity Rarity { get; } = SpecialAbilityEvent.Rarity.Legendary;
    public string DisplayName { get; } = "반사 <color=#4dab99>(전설)</color>";
    public string Description { get; } = "공격을 받을 시 공격자에게 최대 100의 데미지를 반사합니다.";
    public SpecialAbilityEvent Event { get; set; }
}