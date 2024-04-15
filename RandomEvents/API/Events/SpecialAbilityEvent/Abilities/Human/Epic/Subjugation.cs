using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using RandomEvents.API.Events.SpecialAbilityEvent.Enums;
using UnityEngine;

namespace RandomEvents.API.Events.SpecialAbilityEvent.Abilities.Human.Epic;

public class Subjugation : IAbility
{
    public object Clone()
    {
        return new Subjugation();
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

        var random = Random.Range(0, 100);

        if (random >= 14) return;

        ev.Attacker.CurrentItem = null;
    }

    public AbilityType Type { get; } = AbilityType.EPIC_SUBJUGATION;
    public Player Player { get; set; }
    public AbilityRole Role { get; } = AbilityRole.Human;
    public SpecialAbilityEvent.Rarity Rarity { get; } = SpecialAbilityEvent.Rarity.Epic;
    public string DisplayName { get; } = "제압";
    public string Description { get; } = "피격당할 시 14% 확률로 상대방이 무장해제됩니다.";
    public SpecialAbilityEvent Event { get; set; }
}