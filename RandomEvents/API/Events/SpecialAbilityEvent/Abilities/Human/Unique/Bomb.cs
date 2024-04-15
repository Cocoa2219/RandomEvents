using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.Events.EventArgs.Player;
using MEC;
using RandomEvents.API.Events.SpecialAbilityEvent.Enums;

namespace RandomEvents.API.Events.SpecialAbilityEvent.Abilities.Human.Unique;

public class Bomb : IAbility
{
    public object Clone()
    {
        return new Bomb();
    }

    public void RegisterEvents()
    {
        Exiled.Events.Handlers.Player.Dying += OnDying;
    }

    public void UnregisterEvents()
    {
        Exiled.Events.Handlers.Player.Dying -= OnDying;
    }

    private void OnDying(DyingEventArgs ev)
    {
        if (ev.Player != Player) return;

        var wasInLift = ev.Player.Lift != null;
        var liftType = wasInLift ? ev.Player.Lift.Type : ElevatorType.Unknown;

        var pos = ev.Player.Position;

        Timing.CallDelayed(3f, () =>
        {
            if (liftType != ElevatorType.Unknown)
            {
                pos = Lift.Get(liftType).Position;
            }

            var grenade = (ExplosiveGrenade)Item.Create(ItemType.GrenadeHE);
            grenade.FuseTime = 0.1f;
            grenade.SpawnActive(pos);
        });

    }

    public AbilityType Type { get; } = AbilityType.UNIQUE_BOMB;
    public Player Player { get; set; }
    public AbilityRole Role { get; } = AbilityRole.Human;
    public SpecialAbilityEvent.Rarity Rarity { get; } = SpecialAbilityEvent.Rarity.Unique;
    public string DisplayName { get; } = "폭탄";
    public string Description { get; } = "사망 3초 후 자폭을 통해 주변의 모든 플레이어를 죽입니다.";
    public SpecialAbilityEvent Event { get; set; }
}