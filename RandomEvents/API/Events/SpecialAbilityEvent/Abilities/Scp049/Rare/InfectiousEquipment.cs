using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Scp049;
using RandomEvents.API.Events.SpecialAbilityEvent.Enums;
using UnityEngine;

namespace RandomEvents.API.Events.SpecialAbilityEvent.Abilities.Scp049.Rare;

public class InfectiousEquipment : IAbility
{
    public object Clone()
    {
        return new InfectiousEquipment();
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

        var random = Random.Range(0, NegetiveEffects.Count);

        ev.Target.SyncEffect(new Effect(NegetiveEffects[random], 10));
    }

    public AbilityType Type { get; } = AbilityType.SCP_049_INFECTIOUS_EQUIPMENT;
    public Player Player { get; set; }
    public AbilityRole Role { get; } = AbilityRole.Scp049;
    public SpecialAbilityEvent.Rarity Rarity { get; } = SpecialAbilityEvent.Rarity.Rare;
    public string DisplayName { get; } = "감염 장비";
    public string Description { get; } = "공격 시 대상에게 랜덤한 디버프를 10초 간 추가로 부여합니다.";
    public SpecialAbilityEvent Event { get; set; }

    private readonly List<EffectType> NegetiveEffects = [EffectType.AmnesiaItems, EffectType.AmnesiaVision, EffectType.Asphyxiated, EffectType.Bleeding, EffectType.Blinded, EffectType.Burned, EffectType.Concussed, EffectType.Corroding, EffectType.Deafened, EffectType.Disabled, EffectType.Hemorrhage, EffectType.Poisoned, EffectType.SinkHole, EffectType.Stained, EffectType.InsufficientLighting];
}