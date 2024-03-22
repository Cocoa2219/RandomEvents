using System;
using System.Collections;
using System.Collections.Generic;
using Exiled.API.Features;
using MEC;
using RandomEvents.API.Interfaces;
using UnityEngine;
using Random = UnityEngine.Random;

namespace RandomEvents.API.Events.GiveawayEvent;

public class GiveawayEvent : IEvent
{
    public string Name { get; } = "GiveawayEvent";
    public string DisplayName { get; } = "기브어웨이";
    public string Description { get; } = "1분마다 랜덤으로 아이템이 주어집니다.";

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

    private IEnumerator<float> GiveawayCoroutine()
    {
        while (!Round.IsEnded)
        {
            yield return Timing.WaitForSeconds(60f);
            foreach (var player in Player.List)
            {
                if (player.IsScp || !player.IsAlive) continue;
                if (player.Items.Count >= 8) continue;

                player.AddItem(GetRandomItem());
            }
        }

    }

    public void Run()
    {
        Timing.RunCoroutine(GiveawayCoroutine());
    }

    public void RegisterEvents()
    {

    }

    public void UnregisterEvents()
    {

    }
}