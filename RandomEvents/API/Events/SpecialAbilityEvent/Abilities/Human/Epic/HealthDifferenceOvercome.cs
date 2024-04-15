using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using MEC;
using RandomEvents.API.Events.SpecialAbilityEvent.Enums;
using UnityEngine;

namespace RandomEvents.API.Events.SpecialAbilityEvent.Abilities.Human.Epic;

public class HealthDifferenceOvercome : IAbility
{
    public object Clone()
    {
        return new HealthDifferenceOvercome();
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

        var diff = Mathf.Abs(ev.Attacker.Health - ev.Player.Health);

        diff /= 20;

        Event.AddPlayerStats(ev.Player, new PlayerStatus(0, diff, 0));
        Timing.CallDelayed(0.1f, () => Event.AddPlayerStats(ev.Player, new PlayerStatus(0, -diff, 0)));
    }

    public AbilityType Type { get; } = AbilityType.EPIC_HEALTH_DIFFERENCE_OVERCOME;
    public Player Player { get; set; }
    public AbilityRole Role { get; } = AbilityRole.Human;
    public SpecialAbilityEvent.Rarity Rarity { get; } = SpecialAbilityEvent.Rarity.Epic;
    public string DisplayName { get; } = "체력 차이 극복 <color=#9A6DD7>(에픽)</color>";
    public string Description { get; } = "체력 차이에 따라 데미지가 증가합니다.";
    public SpecialAbilityEvent Event { get; set; }
}