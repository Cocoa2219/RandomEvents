using System.Collections.Generic;
using System.Linq;
using Exiled.API.Features;
using Exiled.API.Features.Roles;
using MEC;
using PlayerRoles;
using RandomEvents.API.Events.SpecialAbilityEvent.Enums;
using UnityEngine;

namespace RandomEvents.API.Events.SpecialAbilityEvent.Abilities.Scp173.Epic;

public class Darkness : IAbility
{
    public object Clone()
    {
        return new Darkness();
    }

    public void RegisterEvents()
    {
        if (Player.Role != RoleTypeId.Scp173) return;

        Player.Role.As<Scp173Role>().ObserversTracker.OnObserversChanged += OnObserversChanged;
    }

    public void UnregisterEvents()
    {
        if (Player.Role != RoleTypeId.Scp173) return;

        Player.Role.As<Scp173Role>().ObserversTracker.OnObserversChanged -= OnObserversChanged;

        if (_darknessCooldownCoroutine.IsRunning)
            Timing.KillCoroutines(_darknessCooldownCoroutine);
    }

    private void OnObserversChanged(int prev, int current)
    {
        if (current == 0) return;
        if (_isCooldown) return;

        var rooms = Room.List.Where(x => Vector3.Distance(Player.Position, x.Position) < 20f);

        foreach (var room in rooms)
        {
            room.TurnOffLights(10f);
        }

        _darknessCooldownCoroutine = Timing.RunCoroutine(DarknessCooldownCoroutine());
    }

    private IEnumerator<float> DarknessCooldownCoroutine()
    {
        _isCooldown = true;
        yield return Timing.WaitForSeconds(60f);
        _isCooldown = false;
    }

    public AbilityType Type { get; } = AbilityType.SCP_173_DARKNESS;
    public Player Player { get; set; }
    public AbilityRole Role { get; } = AbilityRole.Scp173;
    public SpecialAbilityEvent.Rarity Rarity { get; } = SpecialAbilityEvent.Rarity.Epic;
    public string DisplayName { get; } = "암흑";
    public string Description { get; } = "인간이 바라볼 시 주변 구역이 정전됩니다.";
    public SpecialAbilityEvent Event { get; set; }

    private bool _isCooldown;
    private CoroutineHandle _darknessCooldownCoroutine;
}