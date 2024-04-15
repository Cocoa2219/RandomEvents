using Exiled.API.Features;
using RandomEvents.API.Events.SpecialAbilityEvent.Enums;

namespace RandomEvents.API.Events.SpecialAbilityEvent.Abilities.Human.Unique;

public class Shield : IAbility
{
    public object Clone()
    {
        return new Shield();
    }

    public void RegisterEvents()
    {
        Exiled.Events.Handlers.Player.ItemAdded += OnItemAdded;
        Exiled.Events.Handlers.Player.ItemRemoved += OnItemRemoved;
    }

    public void UnregisterEvents()
    {
        Exiled.Events.Handlers.Player.ItemAdded -= OnItemAdded;
        Exiled.Events.Handlers.Player.ItemRemoved -= OnItemRemoved;
    }

    private void OnItemAdded(Exiled.Events.EventArgs.Player.ItemAddedEventArgs ev)
    {
        if (ev.Player != Player)
            return;

        _itemCount++;
        Event.AddPlayerStats(Player, new PlayerStatus(0, 7.5f, 0));
    }

    private void OnItemRemoved(Exiled.Events.EventArgs.Player.ItemRemovedEventArgs ev)
    {
        if (ev.Player != Player)
            return;

        _itemCount--;
        Event.AddPlayerStats(Player, new PlayerStatus(0, -7.5f, 0));
    }

    public AbilityType Type { get; } = AbilityType.UNIQUE_DEFENSE_SHIELD;
    public Player Player { get; set; }
    public AbilityRole Role { get; } = AbilityRole.Human;
    public SpecialAbilityEvent.Rarity Rarity { get; } = SpecialAbilityEvent.Rarity.Unique;
    public string DisplayName { get; } = "쉴드 <color=#FFDC41>(유니크)</color>";
    public string Description { get; } = "보유한 아이템 수 1개 당 방어력를 7.5% 증가시킵니다.";
    public SpecialAbilityEvent Event { get; set; }

    private byte _itemCount;
}