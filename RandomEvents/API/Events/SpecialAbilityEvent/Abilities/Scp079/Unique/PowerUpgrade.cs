using System.Linq;
using Exiled.API.Features;
using Exiled.API.Features.Roles;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Player;
using PlayerRoles;
using RandomEvents.API.Events.SpecialAbilityEvent.Enums;
using Map = Exiled.Events.Handlers.Map;

namespace RandomEvents.API.Events.SpecialAbilityEvent.Abilities.Scp079.Unique;

public class PowerUpgrade : IAbility
{
    public object Clone()
    {
        return new PowerUpgrade();
    }

    public void RegisterEvents()
    {
        Map.GeneratorActivating += OnGeneratorEngaged;

        _oldRegenerationPerTier = Player.Role.As<Scp079Role>().AuxManager._regenerationPerTier;
    }

    public void UnregisterEvents()
    {
        Map.GeneratorActivating -= OnGeneratorEngaged;

        Player.Role.As<Scp079Role>().AuxManager._regenerationPerTier = _oldRegenerationPerTier;
    }

    private void OnGeneratorEngaged(GeneratorActivatingEventArgs ev)
    {
        var multiplier = Generator.List.Count(x => x.IsEngaged) * 0.5f;

        if (Player.Role != RoleTypeId.Scp079) return;

        foreach (var regeneration in Player.Role.As<Scp079Role>().AuxManager._regenerationPerTier)
        {
            var value = regeneration * (1 + multiplier);
            Player.Role.As<Scp079Role>().AuxManager._regenerationPerTier[Player.Role.As<Scp079Role>().AuxManager._regenerationPerTier.IndexOf(regeneration)] = value;
        }
    }

    public AbilityType Type { get; } = AbilityType.SCP_079_POWER_UPGRADE;
    public Player Player { get; set; }
    public AbilityRole Role { get; } = AbilityRole.Scp079;
    public SpecialAbilityEvent.Rarity Rarity { get; } = SpecialAbilityEvent.Rarity.Unique;
    public string DisplayName { get; } = "POWER UPGRADE";
    public string Description { get; } = "가동된 발전기 1개 당 보조 전력 회복 속도가 50% 증가합니다.";
    public SpecialAbilityEvent Event { get; set; }

    private float[] _oldRegenerationPerTier;
}