using Exiled.API.Features;
using Exiled.API.Features.Roles;
using PlayerRoles;
using RandomEvents.API.Events.SpecialAbilityEvent.Enums;

namespace RandomEvents.API.Events.SpecialAbilityEvent.Abilities.Scp079.Special;

public class PowerOverwhelming : IAbility
{
    public object Clone()
    {
        return new PowerOverwhelming();
    }

    public void RegisterEvents()
    {
        if (Player.Role != RoleTypeId.Scp079) return;

        _oldRegenerationPerTier = Player.Role.As<Scp079Role>().AuxManager._regenerationPerTier;

        foreach (var regeneration in Player.Role.As<Scp079Role>().AuxManager._regenerationPerTier)
        {
            var value = regeneration * 1.3f;
            Player.Role.As<Scp079Role>().AuxManager._regenerationPerTier[Player.Role.As<Scp079Role>().AuxManager._regenerationPerTier.IndexOf(regeneration)] = value;
        }

        Player.Role.As<Scp079Role>().Level = 5;
    }

    public void UnregisterEvents()
    {
        if (Player.Role != RoleTypeId.Scp079) return;

        Player.Role.As<Scp079Role>().AuxManager._regenerationPerTier = _oldRegenerationPerTier;
    }

    public AbilityType Type { get; } = AbilityType.SCP_079_POWER_OVERWHELMING;
    public Player Player { get; set; }
    public AbilityRole Role { get; } = AbilityRole.Scp079;
    public SpecialAbilityEvent.Rarity Rarity { get; } = SpecialAbilityEvent.Rarity.Special;
    public string DisplayName { get; } = "POWER OVERWHELMING";
    public string Description { get; } = "5티어에서 시작하고 보조 전력 재생이 30% 증가합니다.";
    public SpecialAbilityEvent Event { get; set; }

    private float[] _oldRegenerationPerTier;
}