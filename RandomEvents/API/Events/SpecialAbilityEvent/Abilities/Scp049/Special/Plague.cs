using System.Linq;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Scp049;
using RandomEvents.API.Events.SpecialAbilityEvent.Enums;
using UnityEngine;

namespace RandomEvents.API.Events.SpecialAbilityEvent.Abilities.Scp049.Special;

public class Plague : IAbility
{
    public object Clone()
    {
        return new Plague();
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

        foreach (var player in Exiled.API.Features.Player.List.Where(x => Vector3.Distance(ev.Target.Position, x.Position) < 5f && !x.IsScp))
        {
            player.EnableEffect(EffectType.CardiacArrest, 1, 5);
        }
    }

    public AbilityType Type { get; } = AbilityType.SCP_049_PLAGUE;
    public Player Player { get; set; }
    public AbilityRole Role { get; } = AbilityRole.Scp049;
    public SpecialAbilityEvent.Rarity Rarity { get; } = SpecialAbilityEvent.Rarity.Special;
    public string DisplayName { get; } = "역병";
    public string Description { get; } = "심장 마비 효과를 받은 대상 주변의 5m 내 모든 인원에게 심장 마비 효과를 5초 간 전파합니다.";
    public SpecialAbilityEvent Event { get; set; }
}