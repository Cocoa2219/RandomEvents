using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using PlayerRoles;
using RandomEvents.API.Events.SpecialAbilityEvent.Enums;

namespace RandomEvents.API.Events.SpecialAbilityEvent.Abilities.Scp096.Rare;

public class BloodLust : IAbility
{
    public object Clone()
    {
        return new BloodLust();
    }

    public void RegisterEvents()
    {
        Exiled.Events.Handlers.Player.Dying += OnDying;
    }

    public void UnregisterEvents()
    {
        Exiled.Events.Handlers.Player.Dying -= OnDying;
    }

    private void OnDying(DyingEventArgs ev)
    {
        if (ev.Attacker is null) return;
        if (ev.Attacker != Player) return;
        if (ev.Attacker.Role != RoleTypeId.Scp096) return;

        Player.Health += Player.MaxHealth * 0.012f;
    }

    public AbilityType Type { get; } = AbilityType.SCP_096_BLOODLUST;
    public Player Player { get; set; }
    public AbilityRole Role { get; } = AbilityRole.Scp096;
    public SpecialAbilityEvent.Rarity Rarity { get; } = SpecialAbilityEvent.Rarity.Rare;
    public string DisplayName { get; } = "피의 갈망";
    public string Description { get; } = "적 처치 시 HP 1.2%를 회복합니다.";
    public SpecialAbilityEvent Event { get; set; }
}