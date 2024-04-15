using System.Collections.Generic;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using MEC;
using RandomEvents.API.Events.SpecialAbilityEvent.Enums;

namespace RandomEvents.API.Events.SpecialAbilityEvent.Abilities.Human.Unique;

public class Crescendo : IAbility
{
    public object Clone()
    {
        return new Crescendo();
    }

    public void RegisterEvents()
    {
        Exiled.Events.Handlers.Player.Hurting += OnHurting;
    }

    public void UnregisterEvents()
    {
        Exiled.Events.Handlers.Player.Hurting -= OnHurting;
    }

    private void OnHurting(HurtingEventArgs ev)
    {
        if (ev.Attacker != Player) return;

        if (_hitCountCoroutine.IsRunning)
        {
            Timing.KillCoroutines(_hitCountCoroutine);
        }

        _hitCountCoroutine = Timing.RunCoroutine(IncreaseDamage());
    }

    private IEnumerator<float> IncreaseDamage()
    {
        _hitCount++;

        var damage = _hitCount * 0.1f;
        _damage += damage;

        Event.AddPlayerStats(Player, new PlayerStatus(damage, 0, 0));

        yield return Timing.WaitForSeconds(3f);

        Event.AddPlayerStats(Player, new PlayerStatus(-_damage, 0, 0));

        _hitCount = 0;
        _damage = 0;
    }

    public AbilityType Type { get; } = AbilityType.UNIQUE_CRESCENDO;
    public Player Player { get; set; }
    public AbilityRole Role { get; } = AbilityRole.Human;
    public SpecialAbilityEvent.Rarity Rarity { get; } = SpecialAbilityEvent.Rarity.Unique;
    public string DisplayName { get; } = "크레센도 <color=#FFDC41>(유니크)</color>";
    public string Description { get; } = "적을 연속으로 타격할 수록 공격력이 증가합니다.";
    public SpecialAbilityEvent Event { get; set; }

    private int _hitCount;
    private CoroutineHandle _hitCountCoroutine;
    private float _damage;
}