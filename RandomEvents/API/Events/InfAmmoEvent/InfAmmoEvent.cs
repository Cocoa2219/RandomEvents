using System.Collections.Generic;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using RandomEvents.API.Interfaces;
using Player = Exiled.Events.Handlers.Player;

namespace RandomEvents.API.Events.InfAmmoEvent;

public class InfAmmoEvent : IEvent
{
    public string Name { get; } = "InfAmmoEvent";
    public string DisplayName { get; } = "무한 탄약";
    public string Description { get; } = "총알이 무한으로 주어집니다.";
    public void Run()
    {

    }

    public void RegisterEvents()
    {
        Player.Shooting += OnShooting;
    }

    public void UnregisterEvents()
    {
        Player.Shooting -= OnShooting;
    }

    private void OnShooting(ShootingEventArgs ev)
    {
        ev.Firearm.Ammo = ev.Firearm.MaxAmmo;
    }
}