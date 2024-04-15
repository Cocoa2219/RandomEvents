using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Scp049;
using PlayerStatsSystem;
using RandomEvents.API.Events.SpecialAbilityEvent.Enums;

namespace RandomEvents.API.Events.SpecialAbilityEvent.Abilities.Scp049.Unique;

public class Expertise : IAbility
{
    public object Clone()
    {
        return new Expertise();
    }

    public void RegisterEvents()
    {
        Exiled.Events.Handlers.Scp049.Attacking += OnAttacking;
    }

    public void UnregisterEvents()
    {
        Exiled.Events.Handlers.Scp049.Attacking -= OnAttacking;
    }

    private void OnAttacking(AttackingEventArgs ev)
    {
        if (ev.Player != Player) return;
        ev.Target.Kill(new Scp049DamageHandler(ev.Player.ReferenceHub, -1f, Scp049DamageHandler.AttackType.Instakill));
    }

    public AbilityType Type { get; } = AbilityType.SCP_049_EXPERTISE;
    public Player Player { get; set; }
    public AbilityRole Role { get; } = AbilityRole.Scp049;
    public SpecialAbilityEvent.Rarity Rarity { get; } = SpecialAbilityEvent.Rarity.Unique;
    public string DisplayName { get; } = "노련함 <color=#FF0000>(유니크)</color>";
    public string Description { get; } = "공격 시 대상을 즉시 처치합니다.";
    public SpecialAbilityEvent Event { get; set; }
}