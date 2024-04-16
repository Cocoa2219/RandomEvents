using System.Collections.Generic;
using System.Linq;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Hazards;
using Exiled.API.Features.Roles;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Scp173;
using Hazards;
using MEC;
using RandomEvents.API.Events.SpecialAbilityEvent.Enums;

namespace RandomEvents.API.Events.SpecialAbilityEvent.Abilities.Scp173.Rare;

public class ToxicSubstance : IAbility
{
    public object Clone()
    {
        return new ToxicSubstance();
    }

    public void RegisterEvents()
    {
        Exiled.Events.Handlers.Scp173.PlacingTantrum += OnTantrum;
        Exiled.Events.Handlers.Player.StayingOnEnvironmentalHazard += OnStayingHazard;
    }

    public void UnregisterEvents()
    {
        Exiled.Events.Handlers.Scp173.PlacingTantrum -= OnTantrum;
        Exiled.Events.Handlers.Player.StayingOnEnvironmentalHazard -= OnStayingHazard;
    }

    private void OnStayingHazard(StayingOnEnvironmentalHazardEventArgs ev)
    {
        if (!_tantrumHazards.Contains(ev.Hazard.As<TantrumHazard>().Base)) return;

        if (!_hazardPlayers.Add(ev.Player)) return;
        ev.Player.Hurt(Player, 3f, DamageType.Custom, null, null);
        Timing.CallDelayed(1f, () => { _hazardPlayers.Remove(ev.Player); });
    }

    private void OnTantrum(PlacingTantrumEventArgs ev)
    {
        if (ev.Player != Player) return;

        _tantrumHazards.Add(ev.TantrumHazard);
    }

    public AbilityType Type { get; } = AbilityType.SCP_173_TOXIC_SUBSTANCE;
    public Player Player { get; set; }
    public AbilityRole Role { get; } = AbilityRole.Scp173;
    public SpecialAbilityEvent.Rarity Rarity { get; } = SpecialAbilityEvent.Rarity.Rare;
    public string DisplayName { get; } = "유독 물질";
    public string Description { get; } = "오물 위 적에게 1초 당 3의 피해를 입힙니다.";
    public SpecialAbilityEvent Event { get; set; }

    private readonly List<TantrumEnvironmentalHazard> _tantrumHazards = new();
    private HashSet<Player> _hazardPlayers;
}