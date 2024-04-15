using System.Collections.Generic;
using Exiled.API.Features;
using MEC;
using RandomEvents.API.Events.SpecialAbilityEvent.Enums;

namespace RandomEvents.API.Events.SpecialAbilityEvent.Abilities.Scp0492.Legendary;

public class Undead : IAbility
{
    public object Clone()
    {
        return new Undead();
    }

    public void RegisterEvents()
    {
        _defenseCoroutine = Timing.RunCoroutine(DefenseIncrease());
    }

    public void UnregisterEvents()
    {
        if (_defenseCoroutine.IsRunning)
            Timing.KillCoroutines(_defenseCoroutine);
    }

    private IEnumerator<float> DefenseIncrease()
    {
        while (true)
        {
            yield return Timing.WaitForSeconds(1f);
            if (!(Player.Health < Player.MaxHealth)) continue;
            var healthPercentage = Player.Health / Player.MaxHealth;

            var defenseMultiplier = 0.8f * (1.0f - healthPercentage);

            Event.AddPlayerStatsTime(Player, new PlayerStatus(0, defenseMultiplier, 0), 1f);
        }
    }

    public AbilityType Type { get; } = AbilityType.SCP_049_2_UNDEAD;
    public Player Player { get; set; }
    public AbilityRole Role { get; } = AbilityRole.Scp0492;
    public SpecialAbilityEvent.Rarity Rarity { get; } = SpecialAbilityEvent.Rarity.Legendary;
    public string DisplayName { get; } = "언데드";
    public string Description { get; } = "HP가 감소할 수록 방어력이 최대 80% 증가합니다.";
    public SpecialAbilityEvent Event { get; set; }

    private CoroutineHandle _defenseCoroutine;
}