using Exiled.API.Features;
using RandomEvents.API.Interfaces;
using Server = Exiled.Events.Handlers.Server;

namespace RandomEvents.API.Events.TestEvent;

public class TestEvent : IEvent
{
    public string Name { get; } = "TestEvent";
    public string DisplayName { get; } = "테스트 이벤트";
    public string Description { get; } = "테스트 이벤트입니다.";

    private TestEventHandler EventHandler { get; set; }

    public void Run()
    {
        EventHandler = new TestEventHandler(this);
    }

    public void RegisterEvents()
    {
        LogDebug("이벤트 핸들러를 등록합니다.");
        if (EventHandler == null)
        {
            LogError("이벤트 핸들러가 null 입니다.");
            return;
        }

        Server.RoundStarted += EventHandler.OnRoundStart;
    }

    public void UnregisterEvents()
    {
        LogDebug("이벤트 핸들러를 제거합니다.");
        if (EventHandler == null)
        {
            LogError("이벤트 핸들러가 null 입니다.");
            return;
        }

        Server.RoundStarted -= EventHandler.OnRoundStart;
    }

    public void LogInfo(object message)
    {
        Log.Info($"({Name}) {message}");
    }

    public void LogDebug(object message)
    {
        Log.Debug($"({Name}) {message}");
    }

    public void LogWarn(object message)
    {
        Log.Warn($"({Name}) {message}");
    }

    public void LogError(object message)
    {
        Log.Error($"({Name}) {message}");
    }
}