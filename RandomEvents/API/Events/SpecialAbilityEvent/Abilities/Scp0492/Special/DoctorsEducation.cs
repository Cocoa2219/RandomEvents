using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using MEC;
using PlayerRoles;
using RandomEvents.API.Events.SpecialAbilityEvent.Enums;
using UnityEngine;

namespace RandomEvents.API.Events.SpecialAbilityEvent.Abilities.Scp0492.Special;

public class DoctorsEducation : IAbility
{
    public object Clone()
    {
        return new DoctorsEducation();
    }

    public void RegisterEvents()
    {
        Exiled.Events.Handlers.Player.Hurting += OnHurting;
        Exiled.Events.Handlers.Player.Dying += OnDying;
    }

    public void UnregisterEvents()
    {
        Exiled.Events.Handlers.Player.Hurting -= OnHurting;
        Exiled.Events.Handlers.Player.Dying -= OnDying;
    }

    private void OnHurting(HurtingEventArgs ev)
    {
        if (ev.Attacker is null) return;
        if (ev.Attacker != Player) return;
        if (Player.Role != RoleTypeId.Scp0492) return;

        ev.Player.EnableEffect(EffectType.CardiacArrest, 1, 45);
        _players.Add(ev.Player);
        Timing.CallDelayed(45f, () =>
        {
            _players.Remove(ev.Player);
        });
    }

    private void OnDying(DyingEventArgs ev)
    {
        if (ev.Attacker == null) return;
        if (ev.Attacker != Player) return;
        if (Player.Role != RoleTypeId.Scp0492) return;

        if (_players.Contains(ev.Player))
        {
            Timing.CallDelayed(.1f, () =>
            {
                ev.Player.Role.Set(RoleTypeId.Scp0492, SpawnReason.Revived, RoleSpawnFlags.All);
                ev.Player.MaxHealth = 600;
                ev.Player.Health = 600;
                ev.Player.Teleport(Player.Position);
            });
        }
    }

    public AbilityType Type { get; } = AbilityType.SCP_049_2_DOCTORS_EDUCATION;
    public Player Player { get; set; }
    public AbilityRole Role { get; } = AbilityRole.Scp0492;
    public SpecialAbilityEvent.Rarity Rarity { get; } = SpecialAbilityEvent.Rarity.Special;
    public string DisplayName { get; } = "의사의 교육";
    public string Description { get; }  = "공격 시 대상에게 심장마비 효과가 적용됩니다.";
    public SpecialAbilityEvent Event { get; set; } = null;

    private HashSet<Player> _players = [];
}