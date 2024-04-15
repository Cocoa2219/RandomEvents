using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using PlayerRoles;
using RandomEvents.API.Events.SpecialAbilityEvent.Enums;

namespace RandomEvents.API.Events.SpecialAbilityEvent.Abilities.Scp0492.Epic;

public class Ripping : IAbility
{
    public object Clone()
    {
        return new Ripping();
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

        ev.Player.EnableEffect(EffectType.Bleeding, 1, 10);
    }

    public AbilityType Type { get; } = AbilityType.SCP_049_2_RIPPING;
    public Player Player { get; set; }
    public AbilityRole Role { get; } = AbilityRole.Scp0492;
    public SpecialAbilityEvent.Rarity Rarity { get; } = SpecialAbilityEvent.Rarity.Epic;
    public string DisplayName { get; } = "물어 뜯기";
    public string Description { get; } = "공격 시 대상에게 출혈 효과를 부여합니다.";
    public SpecialAbilityEvent Event { get; set; }
}