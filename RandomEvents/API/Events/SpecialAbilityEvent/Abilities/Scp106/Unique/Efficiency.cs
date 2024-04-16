using Exiled.API.Features;
using Exiled.Events.EventArgs.Scp106;
using RandomEvents.API.Events.SpecialAbilityEvent.Enums;

namespace RandomEvents.API.Events.SpecialAbilityEvent.Abilities.Scp106.Unique;

public class Efficiency : IAbility
{
    public object Clone()
    {
        return new Efficiency();
    }

    public void RegisterEvents()
    {
        Exiled.Events.Handlers.Scp106.Stalking += OnStalking;
        Exiled.Events.Handlers.Scp106.ExitStalking += OnExitStalking;
    }

    public void UnregisterEvents()
    {
        Exiled.Events.Handlers.Scp106.Stalking -= OnStalking;
        Exiled.Events.Handlers.Scp106.ExitStalking -= OnExitStalking;
    }

    private void OnStalking(StalkingEventArgs ev)
    {
        if (ev.Player != Player) return;

        ev.IsAllowed = true;
        _hsDifference = Player.HumeShield;
    }

    private void OnExitStalking(ExitStalkingEventArgs ev)
    {
        if (ev.Player != Player) return;

        _hsDifference = Player.HumeShield - _hsDifference;

        Player.Heal(_hsDifference * 0.25f);
    }

    public AbilityType Type { get; } = AbilityType.SCP_106_EFFICIENCY;
    public Player Player { get; set; }
    public AbilityRole Role { get; } = AbilityRole.Scp106;
    public SpecialAbilityEvent.Rarity Rarity { get; } = SpecialAbilityEvent.Rarity.Unique;
    public string DisplayName { get; } = "효율";
    public string Description { get; } = "스토킹 사용 시 HS 회복량의 25% 만큼 HP를 회복합니다.";
    public SpecialAbilityEvent Event { get; set; }

    private float _hsDifference;
}