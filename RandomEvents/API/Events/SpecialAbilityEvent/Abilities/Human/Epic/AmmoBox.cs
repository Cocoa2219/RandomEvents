using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using MEC;
using RandomEvents.API.Events.SpecialAbilityEvent.Enums;
using UnityEngine;

namespace RandomEvents.API.Events.SpecialAbilityEvent.Abilities.Human.Epic;

public class AmmoBox : IAbility
{
    public object Clone()
    {
        return new AmmoBox();
    }

    public void RegisterEvents()
    {
        Exiled.Events.Handlers.Player.Shooting += OnShooting;
    }

    public void UnregisterEvents()
    {
        Exiled.Events.Handlers.Player.Shooting -= OnShooting;
    }

    private void OnShooting(ShootingEventArgs ev)
    {
        if (ev.Player != Player)
            return;

        var random = Random.Range(0, 100);

        if (random >= 10)
            return;

        Timing.CallDelayed(0.1f, () => ev.Firearm.Ammo += 1);
    }

    public AbilityType Type { get; } = AbilityType.EPIC_AMMO_BOX;
    public Player Player { get; set; }
    public AbilityRole Role { get; } = AbilityRole.Human;
    public SpecialAbilityEvent.Rarity Rarity { get; } = SpecialAbilityEvent.Rarity.Epic;
    public string DisplayName { get; } = "탄약 상자";
    public string Description { get; } = "10% 확률로 탄약을 소모하지 않습니다.";
    public SpecialAbilityEvent Event { get; set; }
}