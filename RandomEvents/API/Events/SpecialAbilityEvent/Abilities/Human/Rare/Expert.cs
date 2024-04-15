using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using MEC;
using RandomEvents.API.Events.SpecialAbilityEvent.Enums;

namespace RandomEvents.API.Events.SpecialAbilityEvent.Abilities.Human.Rare;

public class Expert : IAbility
{
    public object Clone()
    {
        return new Expert();
    }

    public void RegisterEvents()
    {
        Player.SyncEffect(new Effect(EffectType.Scp1853, 0, 4));

        Exiled.Events.Handlers.Player.ReceivingEffect += OnReceivingEffect;
    }

    public void UnregisterEvents()
    {
        Exiled.Events.Handlers.Player.ReceivingEffect -= OnReceivingEffect;

        Player.DisableEffect(EffectType.Scp1853);
    }

    private void OnReceivingEffect(ReceivingEffectEventArgs ev)
    {
        if (ev.Player != Player)
            return;

        Timing.CallDelayed(.1f, () =>
        {
            ev.Player.DisableEffect(EffectType.Poisoned);
        });
    }

    public AbilityType Type { get; } = AbilityType.RARE_EXPERT;
    public Player Player { get; set; }
    public AbilityRole Role { get; } = AbilityRole.Human;
    public SpecialAbilityEvent.Rarity Rarity { get; } = SpecialAbilityEvent.Rarity.Rare;
    public string DisplayName { get; } = "전문가 <color=#529CCA>(레어)</color>";
    public string Description { get; } = "SCP-1853가 최대로 적용되고 독 피해가 무효입니다.";
    public SpecialAbilityEvent Event { get; set; }
}