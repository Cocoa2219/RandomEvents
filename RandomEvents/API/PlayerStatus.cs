using Exiled.API.Features;
using UnityEngine;

namespace RandomEvents.API;

public class PlayerStatus(float attack, float defense, float miss)
{
    public float Attack { get; set; } = attack;
    public float Defense { get; set; } = defense;
    public float Miss { get; set; } = miss;

    public static PlayerStatus operator +(PlayerStatus a, PlayerStatus b)
    {
        return new PlayerStatus(a.Attack + b.Attack, a.Defense + b.Defense, a.Miss + b.Miss);
    }

    public static PlayerStatus operator -(PlayerStatus a, PlayerStatus b)
    {
        return new PlayerStatus(a.Attack - b.Attack, a.Defense - b.Defense, a.Miss - b.Miss);
    }

    public static PlayerStatus operator *(PlayerStatus a, PlayerStatus b)
    {
        return new PlayerStatus(a.Attack * b.Attack, a.Defense * b.Defense, a.Miss * b.Miss);
    }

    public static PlayerStatus operator /(PlayerStatus a, PlayerStatus b)
    {
        return new PlayerStatus(a.Attack / b.Attack, a.Defense / b.Defense, a.Miss / b.Miss);
    }

    public override string ToString()
    {
        return $"Attack : {Attack} / Defense : {Defense} / Miss : {Miss}";
    }

    public static float CalculateDamage(PlayerStatus attacker, PlayerStatus defender, float baseDamage)
    {
        var damage = baseDamage + baseDamage * (attacker.Attack - defender.Defense);

        if (damage < 0f)
            damage = 0f;

        var miss = Random.Range(0f, 1f);

        Log.Debug(miss < defender.Miss
            ? $"Base DMG : {baseDamage} / Attacker : {attacker} / Defender : {defender} / Damage : {damage}, But Missed! ({miss} / {defender.Miss})"
            : $"Base DMG : {baseDamage} / Attacker : {attacker} / Defender : {defender} / Damage : {damage}");

        return miss < defender.Miss ? 0f : damage;
    }
}