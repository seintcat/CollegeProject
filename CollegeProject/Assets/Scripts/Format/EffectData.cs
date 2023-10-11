using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 하나의 효과 데이터
[System.Serializable]
public class EffectData
{
    // 경직시간, (< 0)이면 적용안됨
    public float lockdown;

    // 피해량, (< 0)이면 회복량
    public float damage;

    // 이동속도 관련 값, 기본은 증감
    public float speedValue;

    // 이동속도를 곱하는지 여부(밸류가 음수면 안됨)
    public bool isSpeedMult;

    // 효과 이펙트를 보여주는 프리팹
    public GameObject visual;

    // 효과 데이터
    // ===========================================
    // effectCount(==0)
    // - effectCount 적용없음, effectTime = 효과 지속 시간
    // - effectTime도(<0)시 효과 무한지속
    // 
    // effectCount(>0)
    // - effectCount = 효과 적용 횟수, effectTime = 발동 쿨타임
    // 
    // effectCount(<0)
    // - effectCount = 절대값만큼 스택 이후 발동, effectTime = 스택 소멸시간
    // - effectTime도(<0)시 스택 소멸하지 않음
    // ===========================================
    public float effectTime;
    public int effectCount;

    // 같은 효과 중첩 가능 여부
    public bool isAddible;
}
