using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using RandomEvents.API.Events.SpecialAbilityEvent.Enums;
using UnityEngine;

namespace RandomEvents.API.Events.SpecialAbilityEvent.Abilities.Human.Rare;

public class Lucky : IAbility
{
    public object Clone()
    {
        return new Lucky();
    }

    public void RegisterEvents()
    {
        Exiled.Events.Handlers.Player.InteractingDoor += OnInteractingDoor;
        Exiled.Events.Handlers.Player.InteractingLocker += OnInteractingLocker;
    }

    public void UnregisterEvents()
    {
        Exiled.Events.Handlers.Player.InteractingDoor -= OnInteractingDoor;
        Exiled.Events.Handlers.Player.InteractingLocker -= OnInteractingLocker;
    }

    private void OnInteractingDoor(InteractingDoorEventArgs ev)
    {
        if (ev.Player != Player)
            return;

        if (ev.IsAllowed)
            return;

        var random = Random.Range(0, 100);

        if (random >= 5)
            return;

        ev.IsAllowed = true;
    }

    private void OnInteractingLocker(InteractingLockerEventArgs ev)
    {
        if (ev.Player != Player)
            return;

        if (ev.IsAllowed)
            return;

        var random = Random.Range(0, 100);

        if (random >= 5)
            return;

        ev.IsAllowed = true;
    }

    public AbilityType Type { get; } = AbilityType.RARE_LUCKY;
    public Player Player { get; set; }
    public AbilityRole Role { get; } = AbilityRole.Human;
    public SpecialAbilityEvent.Rarity Rarity { get; } = SpecialAbilityEvent.Rarity.Rare;
    public string DisplayName { get; } = "행운아";
    public string Description { get; } = "문과 락커 상호작용 시 5% 확률로 카드키 없이 오픈합니다.";
    public SpecialAbilityEvent Event { get; set; } = null;
}