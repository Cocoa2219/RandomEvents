using System.Linq;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Scp096;
using MEC;
using RandomEvents.API.Events.SpecialAbilityEvent.Enums;

namespace RandomEvents.API.Events.SpecialAbilityEvent.Abilities.Scp096.Legendary;

public class Trauma : IAbility
{
    public object Clone()
    {
        return new Trauma();
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

        Timing.CallDelayed(.1f, () =>
        {
            foreach (var target in ev.Scp096.Targets)
            {
                _damageIncrease += 0.2f;
                _defenseIncrease += 0.07f;

                Event.AddPlayerStats(Player, new PlayerStatus(0.2f, 0.07f, 0));
            }
        });
    }

    private void OnCalmingDown(CalmingDownEventArgs ev)
    {
        if (ev.Player != Player) return;

        Event.AddPlayerStats(Player, new PlayerStatus(-_damageIncrease, -_defenseIncrease, 0));
        _damageIncrease = 0;
        _defenseIncrease = 0;
    }

    public AbilityType Type { get; } = AbilityType.SCP_096_TRAUMA;
    public Player Player { get; set; }
    public AbilityRole Role { get; } = AbilityRole.Scp096;
    public SpecialAbilityEvent.Rarity Rarity { get; }
    public string DisplayName { get; } = "트라우마";
    public string Description { get; } = "폭주 시 대상 1명 당 방어력이 7% 증가하고 공격력이 20% 증가합니다.";
    public SpecialAbilityEvent Event { get; set; }

    private float _defenseIncrease;
    private float _damageIncrease;
}