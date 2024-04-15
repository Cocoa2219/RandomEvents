using System.Collections.Generic;
using System.Linq;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using MEC;
using RandomEvents.API.Events.SpecialAbilityEvent.Enums;
using UnityEngine;

namespace RandomEvents.API.Events.SpecialAbilityEvent.Abilities.Human.Rare;

public class CriticalHit : IAbility
{
    public object Clone()
    {
        return new CriticalHit();
    }

    public void RegisterEvents()
    {
        Exiled.Events.Handlers.Player.Hurting += OnHurting;
    }

    public void UnregisterEvents()
    {
        Exiled.Events.Handlers.Player.Hurting -= OnHurting;

        foreach (var coroutine in DebuffCoroutine.Where(x => x.IsRunning))
        {
            Timing.KillCoroutines(coroutine);
        }
    }

    private void OnHurting(HurtingEventArgs ev)
    {
        if (ev.Attacker == null || ev.Attacker != Player) return;

        var random = Random.Range(0, 100);

        if (random >= 15) return;
        Timing.RunCoroutine(ApplyDebuff(ev.Player));
    }

    private IEnumerator<float> ApplyDebuff(Player target)
    {
        Event.AddPlayerStats(target, new PlayerStatus(0, -0.2f, 0));
        yield return Timing.WaitForSeconds(20f);
        Event.AddPlayerStats(target, new PlayerStatus(0, 0.2f, 0));
    }

    public AbilityType Type { get; } = AbilityType.RARE_CRITICAL_HIT;
    public Player Player { get; set; }
    public AbilityRole Role { get; } = AbilityRole.Human;
    public SpecialAbilityEvent.Rarity Rarity { get; } = SpecialAbilityEvent.Rarity.Rare;
    public string DisplayName { get; } = "치명타 <color=#529CCA>(레어)</color>";
    public string Description { get; } = "적을 공격할 시 15% 확률로 20초 간 적의 방어력이 -20%가 됩니다.";
    public SpecialAbilityEvent Event { get; set; }

    private List<CoroutineHandle> DebuffCoroutine = [];
}