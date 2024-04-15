using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using MEC;
using RandomEvents.API.Events.SpecialAbilityEvent.Enums;

namespace RandomEvents.API.Events.SpecialAbilityEvent.Abilities.Human.Unique;

public class SurvivalExpert : IAbility
{
    public object Clone()
    {
        return new SurvivalExpert();
    }

    public void RegisterEvents()
    {
        Exiled.Events.Handlers.Player.Dying += OnDying;
    }

    public void UnregisterEvents()
    {
        Exiled.Events.Handlers.Player.Dying -= OnDying;

        if (_cooldown.IsRunning)
        {
            Timing.KillCoroutines(_cooldown);
        }
    }

    private void OnDying(DyingEventArgs ev)
    {
        if (ev.Player != Player) return;

        if (_used) return;

        ev.IsAllowed = false;

        ev.Player.Health = 1f;

        ev.Player.AddAhp(100, 100);

        ev.Player.SyncEffect(new Effect(EffectType.MovementBoost, 5, 20));

        Event.AddPlayerStatsTime(Player, new PlayerStatus(0, 20, 0), 5);

        _cooldown = Timing.RunCoroutine(Cooldown(180f));
    }

    private IEnumerator<float> Cooldown(float time)
    {
        _used = true;

        yield return Timing.WaitForSeconds(time);

        _used = false;
    }

    public AbilityType Type { get; } = AbilityType.UNIQUE_SURVIVAL_EXPERT;
    public Player Player { get; set; }
    public AbilityRole Role { get; } = AbilityRole.Human;
    public SpecialAbilityEvent.Rarity Rarity { get; } = SpecialAbilityEvent.Rarity.Unique;
    public string DisplayName { get; } = "생존 전문가 <color=#FFDC41>(유니크)</color>";
    public string Description { get; } = "사망에 이를 수준의 피해를 받을 시 HP 1, AHP 100을 남긴 채로 생존합니다.";
    public SpecialAbilityEvent Event { get; set; }

    private bool _used;
    private CoroutineHandle _cooldown;
}