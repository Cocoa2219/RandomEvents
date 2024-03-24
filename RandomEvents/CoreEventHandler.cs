using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using RandomEvents.API.Events;
using RandomEvents.API.Interfaces;
using Random = UnityEngine.Random;

namespace RandomEvents;

public class CoreEventHandler(RandomEvents plugin)
{
    private RandomEvents Plugin { get; set; } = plugin;

    public List<IEvent> Events { get; set; } = [];

    private IEvent CurrentEvent { get; set; }

    private Dictionary<IEvent, HashSet<Player>> RandomEvents { get; set; } = new();

    public bool isEventRunning;

    public bool isTestRunning;

    public bool isRerolling;

    public int rerollPlayers;

    public int curRerollPlayers;

    internal void OnWaitingForPlayers()
    {
        Events = Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => typeof(IEvent).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
            .Select(t => (IEvent)Activator.CreateInstance(t)).ToList();

        foreach (var @event in Events)
        {
            Log.Debug($"{@event.Name} 이벤트가 로드되었습니다.");
        }

        PickRandomEvent(3);
    }

    public bool RunEvent(IEvent curEvent)
    {
        if (Round.IsStarted) return false;

        var @event = Events.FirstOrDefault(x => x == curEvent);

        if (@event is null)
        {
            return false;
        }

        Log.Debug($"{curEvent.Name} 선택되었습니다.");
        CurrentEvent = @event;
        return true;
    }

    private void PickRandomEvent(int count)
    {
        for (var i = 0; i < count; i++)
        {
            var randomEvent = Events[Random.Range(0, Events.Count)];
            RandomEvents.Add(randomEvent, []);

            Events.Remove(randomEvent);

            Log.Debug($"{randomEvent.Name} 랜덤 이벤트에 추가되었습니다.");
        }
    }

    public void OnRoundRestart()
    {
        Log.Debug("라운드 재시작, 이벤트 제거 시도 중...");
        CurrentEvent?.UnregisterEvents();
        CurrentEvent = null;

        isEventRunning = false;
        RandomEvents.Clear();

        isTestRunning = false;
    }

    public bool Vote(int index, Player player)
    {
        if (Round.IsStarted || isEventRunning) return false;

        if (index < 0 || index >= RandomEvents.Count) return false;

        foreach (var randomEvent in RandomEvents)
        {
            randomEvent.Value.Remove(player);
        }

        var @event = RandomEvents.ElementAt(index).Key;
        RandomEvents[@event].Add(player);

        RefreshRandomEventHint();

        Log.Debug($"{player.Nickname}님이 {@event.Name} 이벤트에 투표했습니다.");

        return true;
    }

    private void RefreshRandomEventHint()
    {
        Dictionary<Player, IEvent> playerEvents = new();

        foreach (var randomEvent in RandomEvents)
        {
            foreach (var p in randomEvent.Value)
            {
                playerEvents.Add(p, randomEvent.Key);
            }
        }

        foreach (var player in Player.List)
        {
            playerEvents.TryAdd(player, null);
        }

        foreach (var playerEvent in playerEvents)
        {
            var idx = -1;
            var currentIndex = 0;

            // Iterate through RandomEvents to find the index of playerEvent
            foreach (var randomEventKvp in RandomEvents)
            {
                if (randomEventKvp.Key.Equals(playerEvent.Value))
                {
                    idx = currentIndex;
                    break;
                }
                currentIndex++;
            }

            var showName = playerEvent.Value == null ? "ㅤ" : playerEvent.Value.DisplayName;
            var showDesc = playerEvent.Value == null ? "ㅤ" : playerEvent.Value.Description;

            switch (idx)
            {
                case 0:
                    playerEvent.Key.ShowHint($@"<align=right><size=110%><color=#eb4034>[1] {RandomEvents.ElementAt(0).Key.DisplayName} | {RandomEvents[RandomEvents.ElementAt(0).Key].Count}</color>\n[2] <color=#ebd386>{RandomEvents.ElementAt(1).Key.DisplayName}</color> | <color=#ebba86>{RandomEvents[RandomEvents.ElementAt(1).Key].Count}</color>\n[3] <color=#ebd386>{RandomEvents.ElementAt(2).Key.DisplayName}</color> | <color=#ebba86>{RandomEvents[RandomEvents.ElementAt(2).Key].Count}</color>\n\n\n<size=90%><b><color=#ffa15e>{showName}</color></b></size>\n<size=80%><color=#ebd386>{showDesc}</color></size></size><size=130>\n\n</size></align>", 120);
                    break;
                case 1:
                    playerEvent.Key.ShowHint($@"<align=right><size=110%>[1] <color=#ebd386>{RandomEvents.ElementAt(0).Key.DisplayName}</color> | <color=#ebba86>{RandomEvents[RandomEvents.ElementAt(0).Key].Count}</color>\n<color=#eb4034>[2] {RandomEvents.ElementAt(1).Key.DisplayName} | {RandomEvents[RandomEvents.ElementAt(1).Key].Count}</color>\n[3] <color=#ebd386>{RandomEvents.ElementAt(2).Key.DisplayName}</color> | <color=#ebba86>{RandomEvents[RandomEvents.ElementAt(2).Key].Count}</color>\n\n\n<size=90%><b><color=#ffa15e>{showName}</color></b></size>\n<size=80%><color=#ebd386>{showDesc}</color></size></size><size=130>\n\n</size></align>", 120);
                    break;
                case 2:
                    playerEvent.Key.ShowHint($@"<align=right><size=110%>[1] <color=#ebd386>{RandomEvents.ElementAt(0).Key.DisplayName}</color> | <color=#ebba86>{RandomEvents[RandomEvents.ElementAt(0).Key].Count}</color>\n[2] <color=#ebd386>{RandomEvents.ElementAt(1).Key.DisplayName}</color> | <color=#ebba86>{RandomEvents[RandomEvents.ElementAt(1).Key].Count}</color>\n<color=#eb4034>[3] {RandomEvents.ElementAt(2).Key.DisplayName} | {RandomEvents[RandomEvents.ElementAt(2).Key].Count}</color>\n\n\n<size=90%><b><color=#ffa15e>{showName}</color></b></size>\n<size=80%><color=#ebd386>{showDesc}</color></size></size><size=130>\n\n</size></align>", 120);
                    break;
                default:
                    playerEvent.Key.ShowHint(@$"<align=right><size=110%>[1] <color=#ebd386>{RandomEvents.ElementAt(0).Key.DisplayName}</color> | <color=#ebba86>{RandomEvents[RandomEvents.ElementAt(0).Key].Count}</color>\n[2] <color=#ebd386>{RandomEvents.ElementAt(1).Key.DisplayName}</color> | <color=#ebba86>{RandomEvents[RandomEvents.ElementAt(1).Key].Count}</color>\n[3] <color=#ebd386>{RandomEvents.ElementAt(2).Key.DisplayName}</color> | <color=#ebba86>{RandomEvents[RandomEvents.ElementAt(2).Key].Count}</color>\n\n\n<size=90%><b><color=#ffa15e>{showName}</color></b></size>\n<size=80%><color=#ebd386>{showDesc}</color></size></size><size=130>\n\n</size></align>", 120);
                    break;
            }

        }
    }

    public void OnRoundStart()
    {
        if (isEventRunning)
        {
            Log.Debug("이벤트가 이미 실행 중입니다.");
            return;
        }

        foreach (var player in Player.List)
        {
            player.ShowHint("", 2);
        }

        RandomEvents = RandomEvents.OrderByDescending(x => x.Value.Count).ToDictionary(x => x.Key, x => x.Value);

        if (!isTestRunning)
            CurrentEvent = RandomEvents.ElementAt(0).Key;

        isEventRunning = true;

        Log.Debug($"{CurrentEvent.Name} 실행되었습니다.");
        CurrentEvent.Run();
        CurrentEvent.RegisterEvents();

        Map.Broadcast(5, $"<cspace=6px><size=30><b><color=#ebd386>{CurrentEvent.DisplayName}</color></b></size>\n<size=25>{CurrentEvent.Description}</size></cspace>", Broadcast.BroadcastFlags.Normal, true);

        Events.Clear();
    }

    public void OnPlayerVerified(VerifiedEventArgs ev)
    {
        if (isEventRunning || Round.IsStarted) return;

        RefreshRandomEventHint();

        if (isRerolling)
        {
            rerollPlayers = Player.List.Count / 2;
        }
    }
}