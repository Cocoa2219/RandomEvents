using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using PlayerRoles;
using RandomEvents.API.Events.SpecialAbilityEvent.Enums;

namespace RandomEvents.API.Events.SpecialAbilityEvent.Abilities.Scp0492.Rare;

public class TailBite : IAbility
{
    public object Clone()
    {
        return new TailBite();
    }

    public void RegisterEvents()
    {
        Exiled.Events.Handlers.Player.Hurting += OnHurting;
    }

    public void UnregisterEvents()
    {
        Exiled.Events.Handlers.Player.Hurting -= OnHurting;
    }

    private void OnHurting(HurtingEventArgs ev)
    {
        if (ev.Attacker is null) return;
        if (ev.Attacker != Player) return;
        if (Player.Role != RoleTypeId.Scp0492) return;

        ev.Attacker.EnableEffect(EffectType.MovementBoost, 10, 5);
    }

    public AbilityType Type { get; } = AbilityType.SCP_049_2_TAIL_BITE;
    public Player Player { get; set; }
    public AbilityRole Role { get; } = AbilityRole.Scp0492;
    public SpecialAbilityEvent.Rarity Rarity { get; } = SpecialAbilityEvent.Rarity.Rare;
    public string DisplayName { get; } = "꼬리 물기";
    public string Description { get; } = "적 공격 시 이동 속도가 10% 증가합니다.";
    public SpecialAbilityEvent Event { get; set; }
}