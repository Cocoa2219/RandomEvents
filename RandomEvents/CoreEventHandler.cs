using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Exiled.API.Features;
using RandomEvents.API.Events;
using RandomEvents.API.Interfaces;
using Random = UnityEngine.Random;

namespace RandomEvents;

public class CoreEventHandler(RandomEvents plugin)
{
    private RandomEvents Plugin { get; set; } = plugin;

    public List<IEvent> Events { get; set; } = [];

    private IEvent CurrentEvent { get; set; }

    internal void OnWaitingForPlayers()
    {
        Events = Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => typeof(IEvent).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
            .Select(t => (IEvent)Activator.CreateInstance(t)).ToList();

        foreach (var @event in Events)
        {
            Log.Debug($"{@event.Name} 이벤트가 로드되었습니다.");
        }

        // PickRandomEvent();
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

    // private void PickRandomEvent()
    // {
    //     var eventProbabilities = Plugin.Config.EventConfiguration.EventProbabilities;
    //
    //     var totalProbability = eventProbabilities.Values.Sum();
    //
    //     if (totalProbability <= 0)
    //     {
    //         Log.Warn("이벤트 확률의 총 합이 0 이하입니다. 이벤트가 활성화되지 않습니다.");
    //     }
    //
    //     var random = Random.Range(0f, 1f);
    //     Log.Debug($"랜덤 값 : {random}");
    //     Random.InitState((int)DateTime.Now.Ticks);
    //     Log.Debug("랜덤 시드 초기화 중....");
    //
    //     var currentProbability = 0f;
    //
    //     foreach (var eventProbability in eventProbabilities)
    //     {
    //         currentProbability += eventProbability.Value;
    //
    //         if (random <= currentProbability)
    //         {
    //             var eventToRun = Events.Where(e => e.Type == eventProbability.Key).ToList();
    //
    //             if (eventToRun.Count >= 2)
    //             {
    //                 Log.Warn("타입이 같은 이벤트가 2개 이상 존재합니다. 이벤트가 실행되지 않습니다.");
    //                 Events.Clear();
    //                 return;
    //             }
    //
    //             if (eventToRun.Count == 1)
    //             {
    //                 CurrentEvent = eventToRun[0];
    //                 CurrentEvent.Run();
    //                 CurrentEvent.RegisterEvents();
    //
    //                 // Destroy other Event instances
    //                 Events.Clear();
    //                 return;
    //             }
    //
    //             Events.Clear();
    //             Log.Warn("이벤트가 존재하지 않습니다.");
    //             return;
    //         }
    //     }
    //
    //     Log.Info("이벤트가 실행되지 않았습니다.");
    // }

    public void OnRoundRestart()
    {
        Log.Debug("라운드 재시작, 이벤트 제거 시도 중...");
        CurrentEvent?.UnregisterEvents();
        CurrentEvent = null;
    }

    public void OnRoundStart()
    {
        if (CurrentEvent == null) return;

        Log.Debug($"{CurrentEvent.Name} 실행되었습니다.");
        CurrentEvent.Run();
        CurrentEvent.RegisterEvents();

        Events.Clear();
    }
}