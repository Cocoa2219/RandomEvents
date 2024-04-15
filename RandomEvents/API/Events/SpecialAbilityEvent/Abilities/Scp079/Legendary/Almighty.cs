using System;
using Exiled.API.Features;
using RandomEvents.API.Events.SpecialAbilityEvent.Enums;

namespace RandomEvents.API.Events.SpecialAbilityEvent.Abilities.Scp079.Legendary;

public class Almighty : IAbility
{
    public object Clone()
    {
        return new Almighty();
    }

    public void RegisterEvents()
    {
        throw new Exception("현재 휴버트가 핫키 시스템을 리팩토링해서 구현 불가능 능력입니다. 걍 포기하십쇼 휴버트 병신");
    }

    public void UnregisterEvents()
    {
        throw new Exception("현재 휴버트가 핫키 시스템을 리팩토링해서 구현 불가능 능력입니다. 걍 포기하십쇼 휴버트 병신");
    }

    public AbilityType Type { get; } = AbilityType.SCP_079_ALMIGHTY;
    public Player Player { get; set; }
    public AbilityRole Role { get; } = AbilityRole.Scp079;
    public SpecialAbilityEvent.Rarity Rarity { get; } = SpecialAbilityEvent.Rarity.Legendary;
    public string DisplayName { get; } = "ALMIGHTY";
    public string Description { get; } = "현재 구현 불가능.";
    public SpecialAbilityEvent Event { get; set; }
}