using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Roles;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Scp049;
using MEC;
using PlayerRoles;
using RandomEvents.API.Events.SpecialAbilityEvent.Enums;

namespace RandomEvents.API.Events.SpecialAbilityEvent.Abilities.Scp049.Legendary;

public class Designation : IAbility
{
    public object Clone()
    {
        return new Designation();
    }

    public void RegisterEvents()
    {
        Exiled.Events.Handlers.Scp049.ActivatingSense += OnActivatingSense;
        Exiled.Events.Handlers.Player.Dying += OnDying;
    }

    public void UnregisterEvents()
    {
        Exiled.Events.Handlers.Scp049.ActivatingSense -= OnActivatingSense;
        Exiled.Events.Handlers.Player.Dying -= OnDying;
    }

    private void OnActivatingSense(ActivatingSenseEventArgs ev)
    {
        if (ev.Player != Player) return;
        ev.Target.EnableEffect(EffectType.CardiacArrest, 1, 30);
    }

    private void OnDying(DyingEventArgs ev)
    {
        if (ev.Player.ReferenceHub == Player.Role.As<Scp049Role>().SenseAbility.Target)
        {
            Timing.CallDelayed(.1f, () =>
            {
                ev.Player.Role.Set(RoleTypeId.Scp0492, SpawnReason.Revived , RoleSpawnFlags.All);
                ev.Player.MaxHealth = 600;
                ev.Player.Health = 600;
                ev.Player.Teleport(Player.Position);
            });
        }
    }

    public AbilityType Type { get; } = AbilityType.SCP_049_DESIGNATION;
    public Player Player { get; set; }
    public AbilityRole Role { get; } = AbilityRole.Scp049;
    public SpecialAbilityEvent.Rarity Rarity { get; } = SpecialAbilityEvent.Rarity.Legendary;
    public string DisplayName { get; } = "지정";
    public string Description { get; } = "“의사의 감각” 스킬 대상에게 심장 마비 효과를 부여합니다.";
    public SpecialAbilityEvent Event { get; set; }
}