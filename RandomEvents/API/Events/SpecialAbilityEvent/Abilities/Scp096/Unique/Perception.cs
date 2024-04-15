using System.Linq;
using Exiled.API.Features;
using Exiled.API.Features.Roles;
using Exiled.Events.EventArgs.Scp096;
using RandomEvents.API.Events.SpecialAbilityEvent.Enums;

namespace RandomEvents.API.Events.SpecialAbilityEvent.Abilities.Scp096.Unique;

public class Perception : IAbility
{
    public object Clone()
    {
        return new Perception();
    }

    public void RegisterEvents()
    {
        Exiled.Events.Handlers.Scp096.Enraging += OnEnrage;
    }

    public void UnregisterEvents()
    {
        Exiled.Events.Handlers.Scp096.Enraging -= OnEnrage;
    }

    private void OnEnrage(EnragingEventArgs ev)
    {
        if (ev.Player != Player) return;

        foreach (var player in Player.List.Where(x => !x.IsScp))
        {
            Player.Role.As<Scp096Role>().AddTarget(player);
        }
    }

    public AbilityType Type { get; } = AbilityType.SCP_096_PERCEPTION;
    public Player Player { get; set; }
    public AbilityRole Role { get; } = AbilityRole.Scp096;
    public SpecialAbilityEvent.Rarity Rarity { get; } = SpecialAbilityEvent.Rarity.Unique;
    public string DisplayName { get; } = "파악";
    public string Description { get; } = "폭주 시 모든 플레이어가 대상이 됩니다.";
    public SpecialAbilityEvent Event { get; set; }
}