using Exiled.API.Features;
using RandomEvents.API.Interfaces;
using Player = Exiled.Events.Handlers.Player;
using Server = Exiled.Events.Handlers.Server;

namespace RandomEvents.API.Events.SMBEvent;

public class SmbEvent : IEvent
{
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

    public string Name { get; } = "SMBEvent";
    public string DisplayName { get; } = "일심동체 이벤트";
    public string Description { get; } = "일심동체 이벤트입니다. 한 명이 사망할 시 모두 사망합니다.";

    private SmbEventHandler EventHandler { get; set; }

    public void Run()
    {
        EventHandler = new SmbEventHandler(this);
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

        Player.Dying += EventHandler.OnPlayerDying;
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

        Player.Dying -= EventHandler.OnPlayerDying;
    }
}