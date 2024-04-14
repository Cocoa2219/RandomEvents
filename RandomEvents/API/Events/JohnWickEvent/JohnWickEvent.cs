using Exiled.API.Features.DamageHandlers;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.Handlers;
using InventorySystem.Items.Firearms;
using PlayerRoles.PlayableScps.Scp939;
using UnityEngine.UIElements;
using Firearm = Exiled.API.Features.Items.Firearm;
using FirearmDamageHandler = PlayerStatsSystem.FirearmDamageHandler;

namespace RandomEvents.API.Events.JohnWickEvent;

public class JohnWickEvent : Event
{
    public override string Name { get; } = "JohnWickEvent";
    public override string DisplayName { get; } = "존 윅";
    public override string Description { get; } = "권총이 강력해집니다.";

    public override void Run()
    {

    }

    public override void RegisterEvents()
    {
        Player.Hurting += OnHurting;
    }

    public override void UnregisterEvents()
    {
        Player.Hurting -= OnHurting;
    }

    private void OnHurting(HurtingEventArgs ev)
    {
        if (ev.DamageHandler.Is(out FirearmDamageHandler firearmDamageHandler))
        {
            if (IsPistol(firearmDamageHandler.WeaponType))
            {
                ev.Amount *= 4f;
            }
        }
    }

    private bool IsPistol(ItemType weaponType)
    {
        return weaponType is ItemType.GunCOM15 or ItemType.GunCOM18 or ItemType.GunRevolver;
    }
}