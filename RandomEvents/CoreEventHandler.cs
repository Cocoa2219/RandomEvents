using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using MEC;
using RandomEvents.API;
using RandomEvents.API.Events;
using RandomEvents.API.Interfaces;
using Random = UnityEngine.Random;

namespace RandomEvents;

public class CoreEventHandler(RandomEvents plugin)
{
    private RandomEvents Plugin { get; set; } = plugin;

    public List<Event> Events { get; set; } = [];

    private Event CurrentEvent { get; set; }

    private Dictionary<Event, HashSet<Player>> RandomEvents { get; set; } = new();

    public Dictionary<Player, PlayerStatus> PlayerStatuses { get; set; } = new();

    public bool isEventRunning;

    public bool isTestRunning;

    public bool isRerolling;

    public int rerollPlayers;
    public int rerollTime;
    public bool isRerolled;

    private Player rerollPlayer;

    public void SetStats(Player player, PlayerStatus stats)
    {
        PlayerStatuses[player] = stats;
    }

    public void StartRerollVote(Player player)
    {
        isRerolling = true;

        Round.IsLobbyLocked = true;

        Log.Debug("재추첨 투표 시작...");

        RerollPlayers.Clear();

        rerollPlayer = player;

        RerollVote(player);

        rerollPlayers = Player.List.Count / 2;

        Timing.RunCoroutine(RerollCoroutine());
    }

    private IEnumerator<float> RerollCoroutine()
    {
        rerollTime = 20;
        while (rerollTime > 0)
        {
            rerollTime--;
            RefreshRerollBroadcast();
            yield return Timing.WaitForSeconds(1f);
        }

        if (!isRerolling) yield break;

        Map.Broadcast(5, "<size=35px><b><cspace=6px><color=#eb4034>모드 재추첨 투표 실패</color></size></b>\n<cspace=3px><size=25px>모드 재추첨 투표가 실패하였습니다.</size></cspace>", Broadcast.BroadcastFlags.Normal, true);

        Round.IsLobbyLocked = false;
        isRerolled = true;
    }

    public HashSet<Player> RerollPlayers { get; set; } = [];

    public bool RerollVote(Player player)
    {
        if (RerollPlayers.Add(player))
        {
            rerollPlayers = Player.List.Count / 2;

            Log.Debug($"{player.Nickname}님이 재추첨 투표에 찬성했습니다.");
            RefreshRerollBroadcast();

            if (RerollPlayers.Count >= rerollPlayers)
            {
                Map.Broadcast(5, "<size=35px><b><cspace=6px><color=#33cc45>모드 재추첨 투표 성공</color></size></b>\n<cspace=3px><size=25px>모드 재추첨 투표가 성공하였습니다.</size></cspace>", Broadcast.BroadcastFlags.Normal, true);

                RandomEvents.Clear();

                Round.IsLobbyLocked = false;

                var eventsCopy = new List<Event>(Events);

                eventsCopy = eventsCopy.Except(RandomEvents.Keys).ToList();

                for (var i = 0; i < 3; i++)
                {
                    var randomIndex = Random.Range(0, eventsCopy.Count);
                    var randomEvent = eventsCopy[randomIndex];

                    RandomEvents.Add(randomEvent, []);

                    Log.Debug($"{randomEvent.Name} 랜덤 이벤트에 추가되었습니다.");

                    eventsCopy.RemoveAt(randomIndex);
                }

                eventsCopy.Clear();

                isRerolled = true;
                isRerolling = false;
            }

            return true;
        }

        Log.Debug($"{player.Nickname}님이 이미 재추첨 투표에 찬성했습니다. 삭제 ㄱ");
        RefreshRerollBroadcast();
        RerollPlayers.Remove(player);
        return false;
    }

    private void RefreshRerollBroadcast()
    {
        var text =
            $"<size=35px><b><cspace=6px><color=#ebd386>재추첨 투표 진행 중...</color></size></b>\n<cspace=3px><size=25px>{rerollPlayer.CustomName}(이)가 모드 재추첨 투표를 시작했습니다.\n투표하시려면 .rr 을 입력해주세요.\n{rerollTime}초 남았습니다. ( {RerollPlayers.Count} / {rerollPlayers} )</size></cspace>";

        Map.Broadcast(2, text, Broadcast.BroadcastFlags.Normal, true);
    }

    internal void OnWaitingForPlayers()
    {
        Log.Debug("이벤트 로드 시도 중...");

        Events = Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => typeof(Event).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
            .Select(t => (Event)Activator.CreateInstance(t)).ToList();

        Log.Debug($"{Events.Count}개의 이벤트를 찾았습니다.");

        foreach (var @event in Events)
        {
            Log.Debug($"{@event.Name} 이벤트가 로드되었습니다.");
        }

        Log.Debug("3개 이벤트를 선택합니다...");

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
        var eventsCopy = new List<Event>(Events);

        for (var i = 0; i < count; i++)
        {
            var randomIndex = Random.Range(0, eventsCopy.Count);
            var randomEvent = eventsCopy[randomIndex];

            RandomEvents.Add(randomEvent, []);

            Log.Debug($"{randomEvent.Name} 랜덤 이벤트에 추가되었습니다.");

            eventsCopy.RemoveAt(randomIndex);
        }

        eventsCopy.Clear();
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
                    playerEvent.Key.ShowHint($@"<align=left><size=110%>[1] <color=#eb4034>{RandomEvents.ElementAt(0).Key.DisplayName} | {RandomEvents[RandomEvents.ElementAt(0).Key].Count}</color>\n[2] <color=#ebd386>{RandomEvents.ElementAt(1).Key.DisplayName}</color> | <color=#ebba86>{RandomEvents[RandomEvents.ElementAt(1).Key].Count}</color>\n[3] <color=#ebd386>{RandomEvents.ElementAt(2).Key.DisplayName}</color> | <color=#ebba86>{RandomEvents[RandomEvents.ElementAt(2).Key].Count}</color>\n\n\n<size=90%><b><color=#ffa15e>{showName}</color></b></size>\n<size=80%><color=#ebd386>{showDesc}</color></size></size><size=130>\n\n</size></align>", 120);
                    break;
                case 1:
                    playerEvent.Key.ShowHint($@"<align=left><size=110%>[1] <color=#ebd386>{RandomEvents.ElementAt(0).Key.DisplayName}</color> | <color=#ebba86>{RandomEvents[RandomEvents.ElementAt(0).Key].Count}</color>\n[2] <color=#eb4034>{RandomEvents.ElementAt(1).Key.DisplayName} | {RandomEvents[RandomEvents.ElementAt(1).Key].Count}</color>\n[3] <color=#ebd386>{RandomEvents.ElementAt(2).Key.DisplayName}</color> | <color=#ebba86>{RandomEvents[RandomEvents.ElementAt(2).Key].Count}</color>\n\n\n<size=90%><b><color=#ffa15e>{showName}</color></b></size>\n<size=80%><color=#ebd386>{showDesc}</color></size></size><size=130>\n\n</size></align>", 120);
                    break;
                case 2:
                    playerEvent.Key.ShowHint($@"<align=left><size=110%>[1] <color=#ebd386>{RandomEvents.ElementAt(0).Key.DisplayName}</color> | <color=#ebba86>{RandomEvents[RandomEvents.ElementAt(0).Key].Count}</color>\n[2] <color=#ebd386>{RandomEvents.ElementAt(1).Key.DisplayName}</color> | <color=#ebba86>{RandomEvents[RandomEvents.ElementAt(1).Key].Count}</color>\n[3] <color=#eb4034>{RandomEvents.ElementAt(2).Key.DisplayName} | {RandomEvents[RandomEvents.ElementAt(2).Key].Count}</color>\n\n\n<size=90%><b><color=#ffa15e>{showName}</color></b></size>\n<size=80%><color=#ebd386>{showDesc}</color></size></size><size=130>\n\n</size></align>", 120);
                    break;
                default:
                    playerEvent.Key.ShowHint(@$"<align=left><size=110%>[1] <color=#ebd386>{RandomEvents.ElementAt(0).Key.DisplayName}</color> | <color=#ebba86>{RandomEvents[RandomEvents.ElementAt(0).Key].Count}</color>\n[2] <color=#ebd386>{RandomEvents.ElementAt(1).Key.DisplayName}</color> | <color=#ebba86>{RandomEvents[RandomEvents.ElementAt(1).Key].Count}</color>\n[3] <color=#ebd386>{RandomEvents.ElementAt(2).Key.DisplayName}</color> | <color=#ebba86>{RandomEvents[RandomEvents.ElementAt(2).Key].Count}</color>\n\n\n<size=90%><b><color=#ffa15e>{showName}</color></b></size>\n<size=80%><color=#ebd386>{showDesc}</color></size></size><size=130>\n\n</size></align>", 120);
                    break;
            }

        }
    }

    public void OnRoundStart()
    {
        isRerolling = false;

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
        rerollPlayers = Player.List.Count / 2;

        PlayerStatuses.TryAdd(ev.Player, new PlayerStatus(0, 0, 0));

        RefreshRandomEventHint();

        if (isRerolling)
        {
            rerollPlayers = Player.List.Count / 2;
        }
    }

    public void OnPlayerHurting(HurtingEventArgs ev)
    {
        if (ev.Attacker == null || ev.Player == null) return;

        PlayerStatuses.TryAdd(ev.Attacker, new PlayerStatus(0, 0, 0));
        PlayerStatuses.TryAdd(ev.Player, new PlayerStatus(0, 0, 0));

        var damage = PlayerStatus.CalculateDamage(PlayerStatuses[ev.Attacker], PlayerStatuses[ev.Player], ev.Amount);

        ev.Amount = damage;
    }
}