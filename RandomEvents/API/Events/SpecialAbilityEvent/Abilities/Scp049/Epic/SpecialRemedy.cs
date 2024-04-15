using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features;
using MEC;
using RandomEvents.API.Events.SpecialAbilityEvent.Enums;

namespace RandomEvents.API.Events.SpecialAbilityEvent.Abilities.Scp049.Epic;

public class SpecialRemedy : IAbility
{
    public object Clone()
    {
        return new SpecialRemedy();
    }

    public void RegisterEvents()
    {
        SpeedIncreaseCoroutine = Timing.RunCoroutine(SpeedIncrease());
    }

    public void UnregisterEvents()
    {
        if (SpeedIncreaseCoroutine.IsRunning)
            Timing.KillCoroutines(SpeedIncreaseCoroutine);
    }

    private IEnumerator<float> SpeedIncrease()
    {
        while (true)
        {
            yield return Timing.WaitForSeconds(1f);
            // Calculate speed multiplier based on health
            if (!(Player.Health < Player.MaxHealth)) continue;
            var healthPercentage = Player.Health / Player.MaxHealth;

            var speedMultiplier = 0.6f * (1.0f - healthPercentage);

            Player.SyncEffect(new Effect(EffectType.MovementBoost, 0, (byte)(speedMultiplier * 100)));
        }
    }

    public AbilityType Type { get; } = AbilityType.SCP_049_SPECIAL_REMEDY;
    public Player Player { get; set; }
    public AbilityRole Role { get; } = AbilityRole.Scp049;
    public SpecialAbilityEvent.Rarity Rarity { get; } = SpecialAbilityEvent.Rarity.Epic;
    public string DisplayName { get; } = "특별한 치료제";
    public string Description { get; } = "HP가 감소할 수록 이동 속도가 증가합니다.";
    public SpecialAbilityEvent Event { get; set; }

    private CoroutineHandle SpeedIncreaseCoroutine;
}