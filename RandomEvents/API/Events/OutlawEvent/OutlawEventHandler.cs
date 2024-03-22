using Exiled.Events.EventArgs.Player;
using MEC;
using UnityEngine;

namespace RandomEvents.API.Events.OutlawEvent;

public class OutlawEventHandler(OutlawEvent @event)
{
    private OutlawEvent Event { get; } = @event;

    private ItemType GetRandomWeapon()
    {
        Random.InitState((int) (Time.time * 1000));

        ItemType[] gunsArray = new ItemType[]
        {
            ItemType.GunCOM15,
            ItemType.GunE11SR,
            ItemType.GunCrossvec,
            ItemType.GunFSP9,
            ItemType.GunLogicer,
            ItemType.GunCOM18,
            ItemType.GunRevolver,
            ItemType.GunAK,
            ItemType.GunShotgun,
            ItemType.GunCom45,
            ItemType.GunFRMG0,
            ItemType.GunA7,
            ItemType.Jailbird,
            ItemType.ParticleDisruptor,
            ItemType.MicroHID
        };

        return gunsArray[Random.Range(0, gunsArray.Length)];
    }

    public void OnRoleChanging(ChangingRoleEventArgs ev)
    {
        Timing.CallDelayed(.1f, () =>
        {
            if (ev.Player.IsScp || !ev.Player.IsAlive) return;

            ev.Player.AddItem(GetRandomWeapon());
        });
    }
}