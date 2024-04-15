using System.Collections.Generic;
using System.Linq;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Scp106;
using MEC;
using RandomEvents.API.Events.SpecialAbilityEvent.Enums;
using UnityEngine;

namespace RandomEvents.API.Events.SpecialAbilityEvent.Abilities.Scp106.Special;

public class PhaseShift : IAbility
{
    public object Clone()
    {
        return new PhaseShift();
    }

    public void RegisterEvents()
    {
        Exiled.Events.Handlers.Scp106.Stalking += OnStalking;
        Exiled.Events.Handlers.Scp106.ExitStalking += OnExitStalking;
    }

    public void UnregisterEvents()
    {
        Exiled.Events.Handlers.Scp106.Stalking -= OnStalking;
        Exiled.Events.Handlers.Scp106.ExitStalking -= OnExitStalking;

        if (_stalkCoroutine.IsRunning)
            Timing.KillCoroutines(_stalkCoroutine);
    }

    private void OnStalking(StalkingEventArgs ev)
    {
        if (ev.Player != Player) return;

        _stalkCoroutine = Timing.RunCoroutine(PhaseShiftCoroutine());
    }

    private IEnumerator<float> PhaseShiftCoroutine()
    {
        List<Player> targets = [];
        while (true)
        {
            yield return Timing.WaitForSeconds(0.1f);

            if (Physics.Raycast(Player.Position, Vector3.up, out var hit, 50f))
            {
                var obj = hit.collider.gameObject;
                var target = Exiled.API.Features.Player.Get(obj.GetComponentInParent<ReferenceHub>());

                if (target is { IsScp: false })
                {
                    if (targets.Contains(target)) continue;
                    if (targets.Count >= 3) continue;

                    targets.Add(target);
                    target.SyncEffect(new Effect(EffectType.PocketCorroding, 0));
                }
            }

        }
    }

    private void OnExitStalking(ExitStalkingEventArgs ev)
    {
        if (ev.Player != Player) return;

        Timing.KillCoroutines(_stalkCoroutine);
        foreach (var player in Exiled.API.Features.Player.List.Where(x => !x.IsScp && Vector3.Distance(ev.Player.Position, x.Position) < 25f))
        {
            player.SyncEffect(new Effect(EffectType.SinkHole, 6));
        }
    }

    public AbilityType Type { get; } = AbilityType.SCP_106_PHASE_SHIFT;
    public Player Player { get; set; }
    public AbilityRole Role { get; } = AbilityRole.Scp106;
    public SpecialAbilityEvent.Rarity Rarity { get; } = SpecialAbilityEvent.Rarity.Special;
    public string DisplayName { get; } = "유체화";
    public string Description { get; } = "토킹 사용 중 지나친 적 최대 3명을 차원 주머니로 이동시킵니다.";
    public SpecialAbilityEvent Event { get; set; }

    private CoroutineHandle _stalkCoroutine;
}