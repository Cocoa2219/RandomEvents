using Exiled.API.Features;
using Exiled.API.Features.Roles;
using PlayerRoles;
using RandomEvents.API.Events.SpecialAbilityEvent.Enums;

namespace RandomEvents.API.Events.SpecialAbilityEvent.Abilities.Scp079.Epic;

public class Experience : IAbility
{
    public object Clone()
    {
        return new Experience();
    }

    public void RegisterEvents()
    {
        if (Player.Role != RoleTypeId.Scp079) return;

        Player.Role.As<Scp079Role>().Level = 3;
    }

    public void UnregisterEvents()
    {

    }

    public AbilityType Type { get; } = AbilityType.SCP_079_EXPERIENCE;
    public Player Player { get; set; }
    public AbilityRole Role { get; } = AbilityRole.Scp079;
    public SpecialAbilityEvent.Rarity Rarity { get; } = SpecialAbilityEvent.Rarity.Epic;
    public string DisplayName { get; } = "EXPERIENCE";
    public string Description { get; } = "3티어로 시작합니다.";
    public SpecialAbilityEvent Event { get; set; }
}