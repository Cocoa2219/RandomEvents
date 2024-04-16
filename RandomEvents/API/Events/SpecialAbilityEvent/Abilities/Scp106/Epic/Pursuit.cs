using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Scp106;
using MEC;
using RandomEvents.API.Events.SpecialAbilityEvent.Enums;

namespace RandomEvents.API.Events.SpecialAbilityEvent.Abilities.Scp106.Epic;

public class Pursuit : IAbility
{
    public object Clone()
    {
        return new Pursuit();
    }

    public void RegisterEvents()
    {
        Exiled.Events.Handlers.Scp106.ExitStalking += OnExitStalking;
        Exiled.Events.Handlers.Scp106.Teleporting += OnTeleporting;
    }

    public void UnregisterEvents()
    {
        Exiled.Events.Handlers.Scp106.ExitStalking -= OnExitStalking;
        Exiled.Events.Handlers.Scp106.Teleporting -= OnTeleporting;
    }

    private void OnExitStalking(ExitStalkingEventArgs ev)
    {
        if (ev.Player != Player) return;
        ev.IsAllowed = true;

        Player.SyncEffect(new Effect(EffectType.MovementBoost, 7, 20));
    }

    private void OnTeleporting(TeleportingEventArgs ev)
    {
        if (ev.Player != Player) return;

        Timing.CallDelayed(2f, () => { Player.SyncEffect(new Effect(EffectType.MovementBoost, 6, 20)); });
    }

    public AbilityType Type { get; } = AbilityType.SCP_106_PURSUIT;
    public Player Player { get; set; }
    public AbilityRole Role { get; } = AbilityRole.Scp106;
    public SpecialAbilityEvent.Rarity Rarity { get; } = SpecialAbilityEvent.Rarity.Epic;
    public string DisplayName { get; } = "추격";
    public string Description { get; } = "스토킹 및 싱크홀 사용 시 이동속도가 20% 증가합니다.";
    public SpecialAbilityEvent Event { get; set; }
}