using System;
using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Pickups;
using Exiled.Events.EventArgs.Player;
using MEC;
using UnityEngine;
using Random = UnityEngine.Random;

namespace RandomEvents.API.Events.GamblingEvent;

public class GamblingEventHandler(GamblingEvent @event)
{
    private GamblingEvent Event { get; } = @event;

    public void OnDroppedItem(DroppedItemEventArgs ev)
    {
        if (ev.Player.IsScp || !ev.Player.IsAlive) return;

        // Random.InitState((int) (Time.time * 1000));
        if (Random.Range(0, 100) < 2)
        {
            ev.Player.SyncEffect(new Effect(EffectType.SeveredHands, 0, 1, true, true));
            return;
        }

        Pickup.CreateAndSpawn(GetRandomItem(), ev.Pickup.Position, ev.Pickup.Rotation, ev.Player);

        ev.Pickup.Destroy();
    }

    private ItemType GetRandomItem()
    {
        var itemTypes = Enum.GetValues(typeof(ItemType));
        ItemType randomItemType;
        do
        {
            // Random.InitState((int) (Time.time * 1000));
            randomItemType = (ItemType) itemTypes.GetValue(Random.Range(0, itemTypes.Length));
        } while (randomItemType == ItemType.None);

        return randomItemType;
    }
}