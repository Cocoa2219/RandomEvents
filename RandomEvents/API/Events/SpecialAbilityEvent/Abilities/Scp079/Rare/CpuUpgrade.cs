using Exiled.API.Features;
using Exiled.API.Features.Roles;
using PlayerRoles;
using RandomEvents.API.Events.SpecialAbilityEvent.Enums;

namespace RandomEvents.API.Events.SpecialAbilityEvent.Abilities.Scp079.Rare;

public class CpuUpgrade : IAbility
{
    public object Clone()
    {
        return new CpuUpgrade();
    }

    public void RegisterEvents()
    {
        if (Player.Role != RoleTypeId.Scp079) return;

        _oldRegenerationPerTier = Player.Role.As<Scp079Role>().AuxManager._regenerationPerTier;
        foreach (var regeneration in Player.Role.As<Scp079Role>().AuxManager._regenerationPerTier)
        {
            var value = regeneration * 1.2f;
            Player.Role.As<Scp079Role>().AuxManager._regenerationPerTier[Player.Role.As<Scp079Role>().AuxManager._regenerationPerTier.IndexOf(regeneration)] = value;
        }
    }

    public void UnregisterEvents()
    {
        if (Player.Role != RoleTypeId.Scp079) return;

        Player.Role.As<Scp079Role>().AuxManager._regenerationPerTier = _oldRegenerationPerTier;
    }

    public AbilityType Type { get; } = AbilityType.SCP_079_CPU_UPGRADE;
    public Player Player { get; set; }
    public AbilityRole Role { get; } = AbilityRole.Scp079;
    public SpecialAbilityEvent.Rarity Rarity { get; } = SpecialAbilityEvent.Rarity.Rare;
    public string DisplayName { get; } = "CPU UPGRADE";
    public string Description { get; } = "보조 전력 회복 속도를 20% 증가시킵니다.";
    public SpecialAbilityEvent Event { get; set; }

    private float[] _oldRegenerationPerTier;
}