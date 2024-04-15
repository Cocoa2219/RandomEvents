using System.Collections.Generic;
using System.Linq;
using Exiled.API.Features;
using MEC;
using RandomEvents.API.Events.SpecialAbilityEvent.Enums;

namespace RandomEvents.API.Events.SpecialAbilityEvent.Abilities.Scp106.Legendary;

public class Digestion : IAbility
{
    public object Clone()
    {
        return new Digestion();
    }

    public void RegisterEvents()
    {
        _pdCoroutine = Timing.RunCoroutine(PdCoroutine());
    }

    public void UnregisterEvents()
    {
        if (_pdCoroutine.IsRunning)
            Timing.KillCoroutines(_pdCoroutine);
    }

    private IEnumerator<float> PdCoroutine()
    {
        while (true)
        {
            yield return Timing.WaitForSeconds(1f);

            var count = Player.List.Count(x => x.IsInPocketDimension);

            Event.AddPlayerStatsTime(Player, new PlayerStatus(0, 0.05f * count, 0), 1f);
            Player.Heal(count * 5);
        }
    }

    public AbilityType Type { get; } = AbilityType.SCP_106_DIGESTION;
    public Player Player { get; set; }
    public AbilityRole Role { get; } = AbilityRole.Scp106;
    public SpecialAbilityEvent.Rarity Rarity { get; } = SpecialAbilityEvent.Rarity.Legendary;
    public string DisplayName { get; } = "소화";
    public string Description { get; } = "차원 주머니에 있는 플레이어 수에 비례해 스탯이 증가합니다.";
    public SpecialAbilityEvent Event { get; set; }

    private CoroutineHandle _pdCoroutine;
}