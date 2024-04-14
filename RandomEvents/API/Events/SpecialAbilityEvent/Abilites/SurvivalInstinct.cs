using System.Collections.Generic;
using System.Linq;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using LightContainmentZoneDecontamination;
using RandomEvents.API.Events.SpecialAbilityEvent.Enums;
using UnityEngine;

namespace RandomEvents.API.Events.SpecialAbilityEvent.Abilites;

public class SurvivalInstinct : IAbility
{
    public object Clone()
    {
        return new SurvivalInstinct();
    }

    public void RegisterEvents()
    {
        Exiled.Events.Handlers.Player.Dying += OnDying;
    }

    public void UnregisterEvents()
    {
        Exiled.Events.Handlers.Player.Dying -= OnDying;
    }

    private void OnDying(DyingEventArgs ev)
    {
        if (ev.Player != Player) return;

        if (_used) return;

        ev.IsAllowed = false;
        ev.Player.Health = 1;

        if (DecontaminationController.Singleton.IsDecontaminating)
        {
            var zone = Random.Range(0, 2);
            if (zone == 0)
            {
                var spawnableRooms = Room.List.Where(x =>
                    _playerSpawnRooms[ZoneType.HeavyContainment].Contains(x.Type)).ToList();

                var room = spawnableRooms[Random.Range(0, spawnableRooms.Count)];

                ev.Player.Position = room.Position + new Vector3(0, 1, 0);
            }
            else
            {
                var spawnableRooms = Room.List.Where(x =>
                    _playerSpawnRooms[ZoneType.Entrance].Contains(x.Type)).ToList();

                var room = spawnableRooms[Random.Range(0, spawnableRooms.Count)];

                ev.Player.Position = room.Position + new Vector3(0, 1, 0);
            }
        }
        else
        {
            var zone = Random.Range(0, 3);
            switch (zone)
            {
                case 0:
                {
                    var spawnableRooms = Room.List.Where(x =>
                        _playerSpawnRooms[ZoneType.LightContainment].Contains(x.Type)).ToList();

                    var room = spawnableRooms[Random.Range(0, spawnableRooms.Count)];

                    ev.Player.Position = room.Position + new Vector3(0, 1, 0);
                    break;
                }
                case 1:
                {
                    var spawnableRooms = Room.List.Where(x =>
                        _playerSpawnRooms[ZoneType.HeavyContainment].Contains(x.Type)).ToList();

                    var room = spawnableRooms[Random.Range(0, spawnableRooms.Count)];

                    ev.Player.Position = room.Position + new Vector3(0, 1, 0);
                    break;
                }
                default:
                {
                    var spawnableRooms = Room.List.Where(x =>
                        _playerSpawnRooms[ZoneType.Entrance].Contains(x.Type)).ToList();

                    var room = spawnableRooms[Random.Range(0, spawnableRooms.Count)];

                    ev.Player.Position = room.Position + new Vector3(0, 1, 0);
                    break;
                }
            }
        }

        _used = true;
    }

    public AbilityType Type { get; } = AbilityType.EPIC_SURVIVAL_INSTINCT;
    public Player Player { get; set; }
    public AbilityRole Role { get; } = AbilityRole.Human;
    public SpecialAbilityEvent.Rarity Rarity { get; } = SpecialAbilityEvent.Rarity.Epic;
    public string DisplayName { get; } = "생존 본능 <color=#9A6DD7>(에픽)</color>";
    public string Description { get; } = "사망에 이르는 피해를 입었을 때 HP 1을 남기고 랜덤한 장소로 텔레포트됩니다.";
    public SpecialAbilityEvent Event { get; set; }

    private bool _used;

    private readonly Dictionary<ZoneType, RoomType[]> _playerSpawnRooms = new()
    {
        {
            ZoneType.LightContainment,
            [
                RoomType.LczAirlock, RoomType.LczCafe, RoomType.LczCrossing, RoomType.LczCurve, RoomType.LczPlants,
                RoomType.LczStraight, RoomType.LczToilets, RoomType.LczTCross
            ]
        },
        {
            ZoneType.HeavyContainment,
            [RoomType.HczCrossing, RoomType.HczCurve, RoomType.HczHid, RoomType.HczStraight, RoomType.HczTCross]
        },
        {
            ZoneType.Entrance,
            [
                RoomType.EzConference, RoomType.EzCafeteria, RoomType.EzCurve, RoomType.EzStraight, RoomType.EzTCross,
                RoomType.EzCrossing
            ]
        }
    };
}