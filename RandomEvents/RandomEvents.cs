using System;
using Exiled.API.Features;
using Player = Exiled.Events.Handlers.Player;
using Server = Exiled.Events.Handlers.Server;

namespace RandomEvents
{
    public class RandomEvents : Plugin<Configuration>
    {
        public static RandomEvents Instance { get; private set; }

        public override string Name => "RandomEvents";
        public override string Author => "Cocoa";
        public override string Prefix => "RandomEvents";
        public override Version Version { get; } = new(1, 0, 0);

        public CoreEventHandler coreEventHandler { get; private set; }

        public override void OnEnabled()
        {
            base.OnEnabled();

            RegisterEvents();

            Instance = this;
            Log.Info("RandomEvents 플러그인이 모두 \"정상적으로\" 활성화되었습니다.");
        }

        public void RegisterEvents()
        {
            coreEventHandler = new CoreEventHandler(this);

            Server.WaitingForPlayers += coreEventHandler.OnWaitingForPlayers;
            Server.RestartingRound += coreEventHandler.OnRoundRestart;
            Server.RoundStarted += coreEventHandler.OnRoundStart;
            Server.RoundStarted += coreEventHandler.OnRoundStart;

            Player.Verified += coreEventHandler.OnPlayerVerified;
            // Player.Hurting += coreEventHandler.OnPlayerHurting;
        }

        public override void OnDisabled()
        {
            base.OnDisabled();

            UnregisterEvents();

            Instance = null;
            Log.Info("플러그인이 비활성화되었습니다.");
        }

        public void UnregisterEvents()
        {
            Server.WaitingForPlayers -= coreEventHandler.OnWaitingForPlayers;
            Server.RestartingRound -= coreEventHandler.OnRoundRestart;
            Server.RoundStarted -= coreEventHandler.OnRoundStart;

            Player.Verified -= coreEventHandler.OnPlayerVerified;
            // Player.Hurting -= coreEventHandler.OnPlayerHurting;

            coreEventHandler = null;
        }
    }
}