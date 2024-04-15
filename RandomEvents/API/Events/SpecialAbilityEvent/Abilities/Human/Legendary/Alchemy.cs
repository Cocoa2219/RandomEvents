using System;
using Exiled.API.Features;
using Exiled.API.Features.Pickups;
using Exiled.Events.EventArgs.Player;
using RandomEvents.API.Events.SpecialAbilityEvent.Enums;

namespace RandomEvents.API.Events.SpecialAbilityEvent.Abilities.Human.Legendary;

public class Alchemy : IAbility
{
    public object Clone()
    {
        return new Alchemy();
    }

    public void RegisterEvents()
    {
        Exiled.Events.Handlers.Player.DroppedItem += OnDroppedItem;
    }

    public void UnregisterEvents()
    {
        Exiled.Events.Handlers.Player.DroppedItem -= OnDroppedItem;
    }

    public void OnDroppedItem(DroppedItemEventArgs ev)
    {
        if (ev.Player.IsScp || !ev.Player.IsAlive) return;

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
            randomItemType = (ItemType) itemTypes.GetValue(UnityEngine.Random.Range(0, itemTypes.Length));
        } while (randomItemType == ItemType.None);

        return randomItemType;
    }

    public AbilityType Type { get; } = AbilityType.LEGENDARY_ALCHEMY;
    public Player Player { get; set; }
    public AbilityRole Role { get; } = AbilityRole.Human;
    public SpecialAbilityEvent.Rarity Rarity { get; } = SpecialAbilityEvent.Rarity.Legendary;
    public string DisplayName { get; } = "연금술 <color=#4dab99>(전설)</color>";
    public string Description { get; } = "아이템을 버릴 시 랜덤한 아이템으로 변경됩니다.";
    public SpecialAbilityEvent Event { get; set; }
}