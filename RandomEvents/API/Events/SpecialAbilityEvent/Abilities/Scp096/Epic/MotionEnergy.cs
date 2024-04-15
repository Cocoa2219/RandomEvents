using Exiled.API.Features;
using Exiled.Events.EventArgs.Scp096;
using MEC;
using RandomEvents.API.Events.SpecialAbilityEvent.Enums;

namespace RandomEvents.API.Events.SpecialAbilityEvent.Abilities.Scp096.Epic;

public class MotionEnergy : IAbility
{
    public object Clone()
    {
        return new MotionEnergy();
    }

    public void RegisterEvents()
    {
        Exiled.Events.Handlers.Scp096.Enraging += OnEnrage;
        Exiled.Events.Handlers.Scp096.CalmingDown += OnCalmingDown;
    }

    public void UnregisterEvents()
    {
        Exiled.Events.Handlers.Scp096.Enraging -= OnEnrage;
        Exiled.Events.Handlers.Scp096.CalmingDown -= OnCalmingDown;
    }

    private void OnEnrage(EnragingEventArgs ev)
    {
        if (ev.Player != Player) return;

        Event.AddPlayerStats(ev.Player, new PlayerStatus(0, 50, 0));
    }

    private void OnCalmingDown(CalmingDownEventArgs ev)
    {
        if (ev.Player != Player) return;

        Event.AddPlayerStatsTime(ev.Player, new PlayerStatus(0, -65, 0), 10);
        Timing.CallDelayed(10f, () => Event.AddPlayerStats(ev.Player, new PlayerStatus(0, -50, 0)));
    }

    public AbilityType Type { get; } = AbilityType.SCP_096_MOTION_ENERGY;
    public Player Player { get; set; }
    public AbilityRole Role { get; } = AbilityRole.Scp096;
    public SpecialAbilityEvent.Rarity Rarity { get; } = SpecialAbilityEvent.Rarity.Epic;
    public string DisplayName { get; } = "운동 에너지";
    public string Description { get; } = "폭주 중일 시 방어력이 상승하지만, 폭주 종료 시 방어력이 감소합니다.";
    public SpecialAbilityEvent Event { get; set; }
}