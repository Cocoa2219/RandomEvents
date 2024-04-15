using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using RandomEvents.API.Events.SpecialAbilityEvent.Enums;

namespace RandomEvents.API.Events.SpecialAbilityEvent.Abilities.Human.Legendary;

public class HardCounter : IAbility
{
    public object Clone()
    {
        return new HardCounter();
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

        var stats = Event.GetPlayerStats(ev.Player);
        var atk = (stats.Attack + stats.Defense) / 1.5f;

        Event.AddPlayerStatsTime(Player, new PlayerStatus(atk, 0, 0), 0.1f);
    }

    public AbilityType Type { get; } = AbilityType.LEGENDARY_HARD_COUNTER;
    public Player Player { get; set; }
    public AbilityRole Role { get; } = AbilityRole.Human;
    public SpecialAbilityEvent.Rarity Rarity { get; } = SpecialAbilityEvent.Rarity.Legendary;
    public string DisplayName { get; } = "하드 카운터";
    public string Description { get; } = "공격 시 대상의 증가된 공격력과 방어력 만큼 자신의 공격력이 증가합니다.";
    public SpecialAbilityEvent Event { get; set; }
}