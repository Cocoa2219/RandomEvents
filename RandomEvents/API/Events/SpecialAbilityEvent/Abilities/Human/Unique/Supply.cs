using System.Collections.Generic;
using Exiled.API.Features;
using MEC;
using RandomEvents.API.Events.SpecialAbilityEvent.Enums;

namespace RandomEvents.API.Events.SpecialAbilityEvent.Abilities.Human.Unique;

public class Supply : IAbility
{
    public object Clone()
    {
        return new Supply();
    }

    public void RegisterEvents()
    {
        _supplyCoroutine = Timing.RunCoroutine(SupplyItem());
    }

    public void UnregisterEvents()
    {
        if (_supplyCoroutine.IsRunning)
        {
            Timing.KillCoroutines(_supplyCoroutine);
        }
    }

    private IEnumerator<float> SupplyItem()
    {
        while (true)
        {
            yield return Timing.WaitForSeconds(120f);

            var item = ItemTypes[UnityEngine.Random.Range(0, ItemTypes.Count)];

            Player.AddItem(item);
        }
    }

    public AbilityType Type { get; } = AbilityType.UNIQUE_SUPPLY;
    public Player Player { get; set; }
    public AbilityRole Role { get; } = AbilityRole.Human;
    public SpecialAbilityEvent.Rarity Rarity { get; } = SpecialAbilityEvent.Rarity.Unique;
    public string DisplayName { get; } = "보급";
    public string Description { get; } = "120초마다 고가치 아이템을 1개 획득합니다.";
    public SpecialAbilityEvent Event { get; set; }

    private CoroutineHandle _supplyCoroutine;
    private readonly List<ItemType> ItemTypes = [ItemType.MicroHID, ItemType.Jailbird, ItemType.ParticleDisruptor, ItemType.GunFRMG0, ItemType.GunLogicer, ItemType.SCP268, ItemType.GunCom45];
}