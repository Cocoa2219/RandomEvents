using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.Events.EventArgs.Player;
using MEC;
using RandomEvents.API.Events.SpecialAbilityEvent.Enums;
using UnityEngine;

namespace RandomEvents.API.Events.SpecialAbilityEvent.Abilites;

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

        var lift = ev.Player.Lift;

        var pos = ev.Player.Position;

        Timing.CallDelayed(3f, () =>
        {
            if (lift != null) pos = lift.Position;

            var grenade = (ExplosiveGrenade)Item.Create(ItemType.GrenadeHE);
            grenade.FuseTime = 0.1f;
            grenade.SpawnActive(pos);
        });

    }

    public AbilityType Type { get; } = AbilityType.UNIQUE_BOMB;
    public Player Player { get; set; }
    public AbilityRole Role { get; } = AbilityRole.Human;
    public SpecialAbilityEvent.Rarity Rarity { get; } = SpecialAbilityEvent.Rarity.Unique;
    public string DisplayName { get; } = "폭탄 <color=#FFDC41>(유니크)</color>";
    public string Description { get; } = "사망 3초 후 자폭을 통해 주변의 모든 플레이어를 죽입니다.";
    public SpecialAbilityEvent Event { get; set; }
}