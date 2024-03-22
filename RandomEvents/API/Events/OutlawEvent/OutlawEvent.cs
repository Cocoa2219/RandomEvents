using Exiled.API.Features;
using MEC;
using RandomEvents.API.Interfaces;
using UnityEngine;
using Player = Exiled.Events.Handlers.Player;

namespace RandomEvents.API.Events.OutlawEvent;

public class OutlawEvent: IEvent
{
    public void LogInfo(object message)
    {
        Log.Info($"({Name}) {message}");
    }

    public void LogDebug(object message)
    {
        Log.Debug($"({Name}) {message}");
    }

    public void LogWarn(object message)
    {
        Log.Warn($"({Name}) {message}");
    }

    public void LogError(object message)
    {
        Log.Error($"({Name}) {message}");
    }


    public string Name { get; } = "OutlawEvent";
    public string DisplayName { get; } = "무법자";
    public string Description { get; } = "스폰 시 무기가 한 정씩 추가로 랜덤으로 주어집니다.";

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

    public void Run()
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

    public void RegisterEvents()
    {
        Player.ChangingRole += EventHandler.OnRoleChanging;
    }

    public void UnregisterEvents()
    {
        Player.ChangingRole -= EventHandler.OnRoleChanging;
    }
}