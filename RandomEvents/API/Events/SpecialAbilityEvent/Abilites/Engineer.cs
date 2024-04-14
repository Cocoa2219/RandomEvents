using System.Collections.Generic;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using Exiled.Events.EventArgs.Player;
using RandomEvents.API.Events.SpecialAbilityEvent.Enums;

namespace RandomEvents.API.Events.SpecialAbilityEvent.Abilites;

public class Engineer : IAbility
{
    public object Clone()
    {
        return new Engineer();
    }

    public void RegisterEvents()
    {
        Exiled.Events.Handlers.Player.InteractingDoor += OnInteractingDoor;
    }

    public void UnregisterEvents()
    {
        Exiled.Events.Handlers.Player.InteractingDoor -= OnInteractingDoor;
    }

    private void OnInteractingDoor(InteractingDoorEventArgs ev)
    {
        if (ev.Player != Player)
            return;

        if (ev.IsAllowed)
            return;

        if (ev.Door.IsLocked)
        {
            // Event.LogDebug(_interactingCount);
            if (_door != null && _door != ev.Door)
            {
                _interactingCount = 0;
            }

            _interactingCount++;
            _door = ev.Door;

            if (_interactingCount >= 5)
            {
                ev.Door.Unlock();
                _interactingCount = 0;
            }
        }
        else
        {
            _interactingCount = 0;
        }
    }

    public AbilityType Type { get; } = AbilityType.RARE_ENGINEER;
    public Player Player { get; set; }
    public AbilityRole Role { get; } = AbilityRole.Human;
    public SpecialAbilityEvent.Rarity Rarity { get; } = SpecialAbilityEvent.Rarity.Rare;
    public string DisplayName { get; } = "기술자 <color=#529CCA>(레어)</color>";
    public string Description { get; } = "잠긴 문을 연속으로 5회 상호작용할 경우 문이 열립니다.";
    public SpecialAbilityEvent Event { get; set; }

    private int _interactingCount = 0;
    private Door _door;
}