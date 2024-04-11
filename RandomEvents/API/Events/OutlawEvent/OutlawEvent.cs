using Exiled.API.Features;
using MEC;
using RandomEvents.API.Interfaces;
using UnityEngine;
using Player = Exiled.Events.Handlers.Player;

namespace RandomEvents.API.Events.OutlawEvent;

public class OutlawEvent: Event
{
    public override string Name { get; } = "OutlawEvent";
    public override string DisplayName { get; } = "무법자";
    public override string Description { get; } = "스폰 시 무기가 한 정씩 추가로 랜덤으로 주어집니다.";

    private OutlawEventHandler EventHandler { get; set; }

    private ItemType GetRandomWeapon()
    {
        // Random.InitState((int) (Time.time * 1000));

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

    public override void Run()
    {
        EventHandler = new OutlawEventHandler(this);

        Timing.CallDelayed(0.1f, () =>
        {
            foreach (var pl in Exiled.API.Features.Player.List)
            {
                if (!pl.IsScp && pl.IsAlive)
                {
                    pl.AddItem(GetRandomWeapon());
                }
            }
        });
    }

    public override void RegisterEvents()
    {
        Player.ChangingRole += EventHandler.OnRoleChanging;
    }

    public override void UnregisterEvents()
    {
        Player.ChangingRole -= EventHandler.OnRoleChanging;
    }
}