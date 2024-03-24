using System.Linq;
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
    public string DisplayName { get; } = "일심동체";
    public string Description { get; } = "한 명이 사망할 시 모두 사망합니다.";

    public SmbEventHandler EventHandler { get; set; }

    public Exiled.API.Features.Player ChosenPlayer { get; set; }

    public void Run()
    {
        LogDebug("라운드가 시작되었습니다.");

        var playerList = Exiled.API.Features.Player.List.ToList();

        playerList.ShuffleList();

        ChosenPlayer = playerList[0];

        LogInfo($"{ChosenPlayer.CustomName}님이 선택되었습니다.");
    }

    public void RegisterEvents()
    {
        EventHandler = new SmbEventHandler(this);

        LogDebug("이벤트 핸들러를 등록합니다.");
        if (EventHandler == null)
        {
            LogError("이벤트 핸들러가 null 입니다.");
            return;
        }

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

        Player.Dying -= EventHandler.OnPlayerDying;
    }
}