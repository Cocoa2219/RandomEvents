using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using MEC;
using PlayerRoles;
using RandomEvents.API.Events.SpecialAbilityEvent.Enums;
using Random = UnityEngine.Random;
using Server = Exiled.Events.Handlers.Server;

namespace RandomEvents.API.Events.SpecialAbilityEvent;

public class SpecialAbilityEvent : Event
{
    public override string Name { get; } = "SpecialAbilityEvent";
    public override string DisplayName { get; } = "특수 능력";
    public override string Description { get; } = "자신을 더욱 강력하게 만들어줄 능력을 부여받습니다. - 매우, 심각하게, 상당한 시간이 걸릴 것으로 예상됩니다.";

    public List<IAbility> AbilitiesList { get; set; } = [];
    private Dictionary<Player, IAbility> Abilities { get; set; } = new();

    private Dictionary<AbilityRole, List<IAbility>> RoleAbilities { get; set; } = [];
    private Dictionary<Player, CoroutineHandle> BcCoroutines { get; set; } = new();
    private Dictionary<Player, PlayerStatus> PlayerStats { get; set; } = new();

    // TODO: 이거나중에안하면SL개발접음ㅅㄱ

    public override void Run()
    {
        // Using Reflection to get all abilities that implement IAbility
        AbilitiesList = Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => typeof(IAbility).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
            .Select(t => (IAbility)Activator.CreateInstance(t)).ToList();

        // Add to RoleAbilities based on Ability's role
        foreach (var ability in AbilitiesList)
        {
            if (RoleAbilities.ContainsKey(ability.Role))
            {
                RoleAbilities[ability.Role].Add(ability);
            }
            else
            {
                RoleAbilities[ability.Role] = [ability];
            }
        }

        foreach (var player in Player.List)
        {
            GiveAbility(player);
        }
    }

    public override void RegisterEvents()
    {
        Server.RespawningTeam += OnRespawningTeam;
        Exiled.Events.Handlers.Player.ChangingRole += OnChangingRole;
        Exiled.Events.Handlers.Player.Hurting += OnHurting;
    }

    public override void UnregisterEvents()
    {
        Server.RespawningTeam -= OnRespawningTeam;
        Exiled.Events.Handlers.Player.ChangingRole -= OnChangingRole;
        Exiled.Events.Handlers.Player.Hurting -= OnHurting;

        foreach (var ability in Abilities)
        {
            ability.Value.UnregisterEvents();
        }
    }

    private void OnHurting(HurtingEventArgs ev)
    {
        PlayerStats.TryAdd(ev.Player, new PlayerStatus(0, 0, 0));
        if (ev.Attacker == null)
        {
            ev.Amount = PlayerStatus.CalculateDamage(new PlayerStatus(0,0, 0), PlayerStats[ev.Player], ev.Amount);
            return;
        }

        PlayerStats.TryAdd(ev.Attacker, new PlayerStatus(0, 0, 0));

        ev.Amount = PlayerStatus.CalculateDamage(PlayerStats[ev.Attacker], PlayerStats[ev.Player], ev.Amount);
    }

    private void OnChangingRole(ChangingRoleEventArgs ev)
    {
        if (ev.NewRole is RoleTypeId.None or RoleTypeId.Spectator or RoleTypeId.Overwatch or RoleTypeId.Filmmaker)
            return;

        Timing.CallDelayed(.1f, () =>
        {
            GiveAbility(ev.Player);
        });
    }

    private void OnRespawningTeam(RespawningTeamEventArgs ev)
    {
        Timing.CallDelayed(.1f, () =>
        {
            foreach (var player in ev.Players)
            {
                GiveAbility(player);
            }
        });
    }

    private void GiveAbility(Player player)
    {
        LogDebug($"Giving ability to {player.CustomName}...");
        if (Abilities.ContainsKey(player))
        {
            Abilities[player].UnregisterEvents();
            Abilities.Remove(player);
        }

        var rarity = PickRarity();
        var role = GetRole(player.Role);

        LogDebug($"Rarity: {rarity}, Role: {role}");

        if (RoleAbilities.TryGetValue(role, out var roleAbility))
        {
            var abilities = roleAbility.Where(x => x.Rarity == rarity).ToList();
            if (abilities.Count > 0)
            {
                var ability = abilities[Random.Range(0, abilities.Count)];
                Abilities[player] = (IAbility)ability.Clone();
                Abilities[player].Player = player;
                Abilities[player].Event = this;

                LogDebug($"{player.CustomName} has been given {ability.DisplayName} ({ability.Type})");

                if (BcCoroutines.TryGetValue(player, out var handle) && handle.IsRunning)
                {
                    Timing.KillCoroutines(handle);
                }

                BcCoroutines[player] = Timing.RunCoroutine(AbilityBc(player));
            }
        }
    }

    public void GiveAbility(Player player, AbilityType type)
    {
        LogDebug($"Giving ability to {player.CustomName}...");
        if (Abilities.ContainsKey(player))
        {
            Abilities[player].UnregisterEvents();
            Abilities.Remove(player);
        }

        var ability = AbilitiesList.FirstOrDefault(x => x.Type == type);
        if (ability == null)
            return;

        Abilities[player] = (IAbility)ability.Clone();
        Abilities[player].Player = player;
        Abilities[player].Event = this;

        LogDebug($"{player.CustomName} has been given {ability.DisplayName} ({ability.Type})");

        if (BcCoroutines.TryGetValue(player, out var handle) && handle.IsRunning)
        {
            Timing.KillCoroutines(handle);
        }

        BcCoroutines[player] = Timing.RunCoroutine(AbilityBc(player));
    }

    private IEnumerator<float> AbilityBc(Player player)
    {
        for (var i = 0; i < 50; i++)
        {
            player.Broadcast(1, @$"<b><size=35><cspace=6px><color={GetRandomHexColor()}>능력 배정 중...</color></b>\n<size=26>과연...?</size></cspace>", Broadcast.BroadcastFlags.Normal, true);
            yield return Timing.WaitForSeconds(0.1f);
        }

        player.Broadcast(4, "<cspace=6px><size=35><b>당신의 능력은...</b>\n<size=30>과연...</size></cspace>", Broadcast.BroadcastFlags.Normal, true);

        yield return Timing.WaitForSeconds(4f);

        Abilities[player].RegisterEvents();
        player.Broadcast(5, @$"<b><size=35><cspace=6px>{Abilities[player].DisplayName} {GetRarityString(Abilities[player].Rarity)}</b>\n<size=26>{Abilities[player].Description}</size></cspace>", Broadcast.BroadcastFlags.Normal, true);
    }

    private string GetRandomHexColor()
    {
        var r = (byte)Random.Range(0, 256);
        var g = (byte)Random.Range(0, 256);
        var b = (byte)Random.Range(0, 256);

        var hex = "#" + r.ToString("X2") + g.ToString("X2") + b.ToString("X2");

        return hex;
    }

    public enum Rarity
    {
        Rare,
        Epic,
        Unique,
        Legendary,
        Special
    }

    private Rarity PickRarity()
    {
        var randomNumber = Random.value;
        return randomNumber switch
        {
            < 0.396f => Rarity.Rare,
            < 0.693f => Rarity.Epic,
            < 0.891f => Rarity.Unique,
            < 0.99f => Rarity.Legendary,
            _ => Rarity.Special
        };
    }

    public AbilityRole GetRole(RoleTypeId role)
    {
        return role switch
        {
            RoleTypeId.Scp173 => AbilityRole.Scp173,
            RoleTypeId.ClassD => AbilityRole.Human,
            RoleTypeId.Scp106 => AbilityRole.Scp106,
            RoleTypeId.NtfSpecialist => AbilityRole.Human,
            RoleTypeId.Scp049 => AbilityRole.Scp049,
            RoleTypeId.Scientist => AbilityRole.Human,
            RoleTypeId.Scp079 => AbilityRole.Scp079,
            RoleTypeId.ChaosConscript => AbilityRole.Human,
            RoleTypeId.Scp096 => AbilityRole.Scp096,
            RoleTypeId.Scp0492 => AbilityRole.Scp0492,
            RoleTypeId.NtfSergeant => AbilityRole.Human,
            RoleTypeId.NtfCaptain => AbilityRole.Human,
            RoleTypeId.NtfPrivate => AbilityRole.Human,
            RoleTypeId.Tutorial => AbilityRole.Human,
            RoleTypeId.FacilityGuard => AbilityRole.Human,
            RoleTypeId.Scp939 => AbilityRole.Scp939,
            RoleTypeId.ChaosRifleman => AbilityRole.Human,
            RoleTypeId.ChaosMarauder => AbilityRole.Human,
            RoleTypeId.ChaosRepressor => AbilityRole.Human,
            _ => AbilityRole.None
        };
    }

    public string GetRarityString(Rarity rarity)
    {
        return rarity switch
        {
            Rarity.Rare => "<color=#529CCA>(레어)</color>",
            Rarity.Epic => "<color=#9a6dd7>(에픽)</color>",
            Rarity.Unique => "<color=#ffdc41>(유니크)</color>",
            Rarity.Legendary => "<color=#4DAB8D>(레전더리)</color>",
            Rarity.Special => "<color=#E24D7B>(스페셜)</color>",
            _ => "<color=#529CCA>(레어)</color>"
        };
    }

    public void SetPlayerStats(Player player, PlayerStatus status)
    {
        PlayerStats[player] = status;
    }

    public void AddPlayerStats(Player player, PlayerStatus status)
    {
        PlayerStats.TryAdd(player, new PlayerStatus(0, 0, 0));
        PlayerStats[player] += status;
    }

    public void AddPlayerStatsTime(Player player, PlayerStatus status, float time)
    {
        PlayerStats.TryAdd(player, new PlayerStatus(0, 0, 0));
        Timing.RunCoroutine(AddPlayerStatsTimeCoroutine(player, status, time));
    }

    public PlayerStatus GetPlayerStats(Player player)
    {
        return PlayerStats.TryGetValue(player, out var status) ? status : new PlayerStatus(0, 0, 0);
    }

    private IEnumerator<float> AddPlayerStatsTimeCoroutine(Player player, PlayerStatus status, float time)
    {
        PlayerStats[player] += status;
        yield return Timing.WaitForSeconds(time);
        PlayerStats[player] -= status;
    }
}