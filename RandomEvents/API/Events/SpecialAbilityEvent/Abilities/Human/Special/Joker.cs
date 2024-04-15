using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features;
using MEC;
using RandomEvents.API.Events.SpecialAbilityEvent.Enums;
using UnityEngine;

namespace RandomEvents.API.Events.SpecialAbilityEvent.Abilities.Human.Special;

public class Joker : IAbility
{
    public object Clone()
    {
        return new Joker();
    }

    public void RegisterEvents()
    {
        JokerCoroutineHandle = Timing.RunCoroutine(JokerCoroutine());
    }

    public void UnregisterEvents()
    {
        if (RegenerationCoroutineHandle.IsRunning)
            Timing.KillCoroutines(RegenerationCoroutineHandle);
        if (JokerCoroutineHandle.IsRunning)
            Timing.KillCoroutines(JokerCoroutineHandle);
    }

    private IEnumerator<float> JokerCoroutine()
    {
        while (true)
        {
            yield return Timing.WaitForSeconds(40f);
            var random = Random.Range(0, 5);

            switch (random)
            {
                case 0:
                    RegenerationCoroutineHandle = Timing.RunCoroutine(RegenerationCoroutine(40, 15f));
                    break;
                case 1:
                    Event.AddPlayerStatsTime(Player, new PlayerStatus(0, 75, 0), 40);
                    break;
                case 2:
                    Player.SyncEffect(new Effect(EffectType.MovementBoost, 40, 75));
                    break;
                case 3:
                    Event.AddPlayerStatsTime(Player, new PlayerStatus(75, 0, 0), 40);
                    break;
                case 4:
                    RegenerationCoroutineHandle = Timing.RunCoroutine(RegenerationCoroutine(40, 15f));
                    Event.AddPlayerStatsTime(Player, new PlayerStatus(0, 75, 0), 40);
                    Player.SyncEffect(new Effect(EffectType.MovementBoost, 40, 75));
                    Event.AddPlayerStatsTime(Player, new PlayerStatus(75, 0, 0), 40);
                    break;
            }

            yield return Timing.WaitForSeconds(80f);
        }
    }

    private IEnumerator<float> RegenerationCoroutine(int time, float amount)
    {
        for (int i = 0; i < time; i++)
        {
            yield return Timing.WaitForSeconds(1f);
            Player.Heal(Player.MaxHealth * amount, false);
        }
    }

    public AbilityType Type { get; } = AbilityType.SPECIAL_JOKER;
    public Player Player { get; set; }
    public AbilityRole Role { get; } = AbilityRole.Human;
    public SpecialAbilityEvent.Rarity Rarity { get; } = SpecialAbilityEvent.Rarity.Special;
    public string DisplayName { get; } = "조커";
    public string Description { get; } = "시작 40초 이후 다음 능력 중 1개를 부여합니다.";
    public SpecialAbilityEvent Event { get; set; }

    private CoroutineHandle JokerCoroutineHandle;
    private CoroutineHandle RegenerationCoroutineHandle;
}