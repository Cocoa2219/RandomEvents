using System.Collections.Generic;
using Exiled.API.Features;
using MEC;
using RandomEvents.API.Events.SpecialAbilityEvent.Enums;

namespace RandomEvents.API.Events.SpecialAbilityEvent.Abilites;

public class Protection : IAbility
{
    public object Clone()
    {
        return new Protection();
    }

    public void RegisterEvents()
    {
        AHPRegenCoroutine = Timing.RunCoroutine(AHPRegen());
    }

    public void UnregisterEvents()
    {
        Timing.KillCoroutines(AHPRegenCoroutine);
    }

    private IEnumerator<float> AHPRegen()
    {
        while (true)
        {
            yield return Timing.WaitForSeconds(1f);
            Player.AddAhp(5);
        }
    }

    public AbilityType Type { get; } = AbilityType.RARE_PROTECTION;
    public Player Player { get; set; }
    public AbilityRole Role { get; } = AbilityRole.Human;
    public SpecialAbilityEvent.Rarity Rarity { get; } = SpecialAbilityEvent.Rarity.Rare;
    public string DisplayName { get; } = "보호 <color=#529CCA>(레어)</color>";
    public string Description { get; } = "1초당 5 AHP를 회복합니다.";
    public SpecialAbilityEvent Event { get; set; }

    private CoroutineHandle AHPRegenCoroutine;
}