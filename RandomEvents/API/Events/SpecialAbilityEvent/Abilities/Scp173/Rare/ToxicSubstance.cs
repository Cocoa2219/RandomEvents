using System.Collections.Generic;
using System.Linq;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Hazards;
using Exiled.API.Features.Roles;
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
        _angerCoroutine = Timing.RunCoroutine(AngerCoroutine());
        Exiled.Events.Handlers.Scp173.PlacingTantrum += OnTantrum;
    }

    public void UnregisterEvents()
    {
        Exiled.Events.Handlers.Scp173.PlacingTantrum -= OnTantrum;
        if (_angerCoroutine.IsRunning)
            Timing.KillCoroutines(_angerCoroutine);
    }

    private IEnumerator<float> AngerCoroutine()
    {
        while (true)
        {
            yield return Timing.WaitForSeconds(1f);

            foreach (var pl in _tantrumHazards.Where(tantrumHazard => tantrumHazard is not null).Select(tantrumHazard => (EnvironmentalHazard)tantrumHazard).SelectMany(hazard => hazard.AffectedPlayers.Select(Player.Get)))
            {
                pl.Hurt(Player, 3f, DamageType.Custom, null, null);
            }
        }
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

    private CoroutineHandle _angerCoroutine;
    private HashSet<TantrumEnvironmentalHazard> _tantrumHazards = new();
}