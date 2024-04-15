// ReSharper disable InconsistentNaming
namespace RandomEvents.API.Events.SpecialAbilityEvent.Enums;

public enum AbilityType
{
    // Human abilities
    RARE_SWIFTNESS, // 이동 속도 +15%
    RARE_RESISTANCE, // 방어력 +30%
    RARE_ENHANCEMENT, // 공격력 +20%
    RARE_REGENERATION, // 1초 당 HP 2% 회복
    RARE_LUCKY, // 문과 락커 상호작용 시 5% 확률로 카드키 없이 오픈
    RARE_EXPERT, // SCP-1853 최대 효과 적용 & 독 피해 무효
    RARE_ENGINEER, // 잠긴 문 연속 5회 상호작용 시 잠금 해제
    RARE_PROTECTION, // 1초 당 AHP 5 회복 (최대 75)
    RARE_CRITICAL_HIT, // 적 공격 시 15% 확률로 적의 방어력 -20% (20초 간)

    // Epic abilities
    EPIC_HEALTH_DIFFERENCE_OVERCOME, // 공격 시 대상과의 HP % 차이 만큼 공격력 증가
    EPIC_EVASION, // 회피율 +70%
    EPIC_AMMO_BOX, // 10% 확률로 탄약 미소모
    EPIC_ABSORPTION, // 공격 시 공격 피해량의 20% 만큼 AHP 획득 (최대 75)
    EPIC_SURVIVAL_INSTINCT, // 사망에 이를 수준의 피해를 받을 시 HP 1을 남기고 랜덤한 장소로 텔레포트 (1회)
    EPIC_SUBJUGATION, // 피격 시 14% 확률로 상대를 무장 해제

    // Unique abilities
    UNIQUE_CRESCENDO, // 적을 연속으로 타격할 수록 공격력 증가 (최대 300%)
    UNIQUE_BOMB, // 사망 시 그 자리에서 3초 뒤 폭발
    UNIQUE_SURVIVAL_EXPERT, // 사망에 이를 수준의 피해를 받을 시 HP 1, AHP 100을 남긴 채로 생존
    UNIQUE_DEFENSE_SHIELD, // 보유한 아이템 수 1개 당 방어력 +7.5%
    UNIQUE_SUPPLY, // 120초마다 고가치 아이템 1개 획득

    // Legendary abilities
    LEGENDARY_HARD_COUNTER, // 공격 시 대상의 증가된 공격력과 방어력 만큼 자신의 공격력 증가
    LEGENDARY_REFLECTION, // 받는 피해의 40% 만큼을 적에게 반사 (최대 100)
    LEGENDARY_SHADOW, // 주변 20m 이내 적 미존재 시 은신 상태에 돌입
    LEGENDARY_ALCHEMY, // 아이템을 버릴 시 랜덤한 아이템으로 변경

    // Special abilities
    SPECIAL_GOLDEN_BELL, // 받는 피해 최대치 25 고정
    SPECIAL_ONE_HUNDRED_THOUSAND, // 주변 20m 이내 적 1명 당 공격력 +35%, 방어력 +10%
    SPECIAL_JOKER, // 시작 40초 이후 다음 능력 중 1개 부여 (40초 간)

    // SCP-049 abilities
    SCP_049_INFECTIOUS_EQUIPMENT, // 공격 시 대상에게 랜덤한 디버프를 추가로 부여 (10초 간)
    SCP_049_SPECIAL_REMEDY, // HP가 감소할 수록 이동 속도 증가 (최대 60%)
    SCP_049_EXPERTISE, // 공격 시 대상 즉시 처치
    SCP_049_DESIGNATION, // “의사의 감각” 스킬 대상에게 심장 마비 효과 부여 (30초 간)
    SCP_049_PLAGUE, // 심장 마비 효과를 받은 대상 주변 5m 내 모든 인원에게 심장 마비 효과 전파 (5초 간)

    // SCP-049-2 abilities
    SCP_049_2_TAIL_BITE, // 적 공격 시 이동 속도 +10% (5초 간)
    SCP_049_2_RIPPING, // 공격 시 대상에게 출혈 효과 부여 (10초 간)
    SCP_049_2_AWAKENING, // 공격력 +50%, 이동 속도 +10%
    SCP_049_2_UNDEAD, // HP가 감소할 수록 방어력 증가 (최대 80%)
    SCP_049_2_DOCTORS_EDUCATION, // 공격 시 대상에게 심장마비 효과 적용 (45초 간)

    // SCP-079 abilities
    SCP_079_CPU_UPGRADE, // 보조 전력 회복 속도 +20%
    SCP_079_EXPERIENCE, // 3티어 시작
    SCP_079_POWER_UPGRADE, // 가동된 발전기 1개 당 보조 전력 회복 속도 +50%
    SCP_079_ALMIGHTY, // 각 티어 도달 시 각종 상호작용 사용 가능 및 능력치 증가
    SCP_079_POWER_OVERWHELMING, // 5티어 시작. 보조 전력 회복 속도 +30%

    // SCP-096 abilities
    SCP_096_BLOODLUST, // 적 처치 시 HP 1.2% 회복
    SCP_096_MOTION_ENERGY, // 폭주 시 방어력 +50% (폭주 종료 시까지), 폭주 종료 시 방어력 -15% (10초 간)
    SCP_096_PERCEPTION, // 폭주 시 전체 인간을 대상으로 지정
    SCP_096_TRAUMA, // 폭주 시 대상 1명 당 방어력 +7% (폭주 종료 시까지), 공격력 +20% (폭주 종료 시까지)
    SCP_096_IMPENETRABLE, // 공격력 +300%, 이동 속도 +10%, 방어력 +25%

    // SCP-106 abilities
    SCP_106_IRON_SKIN, // 방어력 +10%
    SCP_106_PURSUIT, // 스토킹 및 싱크홀 사용 시 이동속도 +20% (6초 간)
    SCP_106_EFFICIENCY, // 스토킹 사용 시 HS 회복량의 25% 만큼 HP를 회복
    SCP_106_DIGESTION, // 차원 주머니에 존재하는 인원 수에 비례해 1초 당 HP +5, 차원 주머니에 존재하는 인원 1명 당 방어력 +5%
    SCP_106_PHASE_SHIFT, // 스토킹 사용 중 지나친 적 최대 3명을 차원 주머니로 이동, 스토킹에서 벗어날 시 주변 25m 내 인원의 이동 속도 -30% (6초 간)

    // SCP-173 abilities
    SCP_173_TOXIC_SUBSTANCE, // 오물 위 적에게 1초 당 3 피해
    SCP_173_DARKNESS, // 인간이 바라볼 시 주변 구역 정전 (10초 간)
    SCP_173_INTERCHANGE, // HP가 감소할 수록 방어력 증가 (최대 50%)
    SCP_173_TELEPORT, // 적 처치 시 주변 2m 내 적 전원에게 200 피해
    SCP_173_FRAGMENTATION, // HS 파괴 시 주변 구역 정전 (15초 간), 주변 10m 내 적 전원에게 최근 30초 간 누적된 피해에 비례한 피해와 실명 효과 적용

    // SCP-939 abilities
    SCP_939_LIGHTER, // 이동 속도 +20%
    SCP_939_ANGER, // 피격 시 공격력 +5% (5초 간, 최대 100%) 이동 속도 +5%. (5초 간, 최대 50%)
    SCP_939_CONSUME, // 적을 처치할 시 스테미나 35% 회복.
    SCP_939_RUMBLE, // 도약 사용 시 착지 지점에 지진을 일으켜 주변 10m 내 적 전원에게 40 피해 및 이동 속도 -30%. (3초 간)
    SCP_939_PANIC,// 적 처치 시 주변 10m 내 적 전원에게 기억 소거 효과 부여. (5초 간)
}