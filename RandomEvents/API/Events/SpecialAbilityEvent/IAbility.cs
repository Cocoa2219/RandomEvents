using System;
using Exiled.API.Features;
using RandomEvents.API.Events.SpecialAbilityEvent.Enums;

namespace RandomEvents.API.Events.SpecialAbilityEvent;

public interface IAbility : ICloneable
{
    public void RegisterEvents();
    public void UnregisterEvents();
    public AbilityType Type { get; }
    public Player Player { get; set; }
    public AbilityRole Role { get; }
    public SpecialAbilityEvent.Rarity Rarity { get; }
    public string DisplayName { get; }
    public string Description { get; }
    public SpecialAbilityEvent Event { get; set; }
}