using System.Collections.Generic;
using Exiled.API.Features;
using MEC;
using RandomEvents.API.Events.SpecialAbilityEvent.Enums;

namespace RandomEvents.API.Events.SpecialAbilityEvent.Abilities.Human.Rare;

public class Regeneration : IAbility
{
    public object Clone()
    {
        return new Regeneration();
    }

    public void RegisterEvents()
    {
        HealthRegenCoroutine = Timing.RunCoroutine(HealthRegen());
    }

    private IEnumerator<float> HealthRegen()
    {
        while (true)
        {
            yield return Timing.WaitForSeconds(1f);
            Player.Heal(Player.MaxHealth * 0.02f, false);
        }
    }

    public void UnregisterEvents()
    {
        Timing.KillCoroutines(HealthRegenCoroutine);
    }

    public AbilityType Type { get; } = AbilityType.RARE_REGENERATION;
    public Player Player { get; set; }
    public AbilityRole Role { get; } = AbilityRole.Human;
    public SpecialAbilityEvent.Rarity Rarity { get; } = SpecialAbilityEvent.Rarity.Rare;
    public string DisplayName { get; } = "재생";
    public string Description { get; } = "체력이 1초당 전체 체력의 2%를 회복합니다.";
    public SpecialAbilityEvent Event { get; set; }

    private CoroutineHandle HealthRegenCoroutine;
}