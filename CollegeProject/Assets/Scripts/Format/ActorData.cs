using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 하나의 액터 데이터
[CreateAssetMenu(fileName = "Name(Data)", menuName = "menu/Actor Data", order = 1)]
public class ActorData : ScriptableObject
{
    // 최대체력
    public float maxHp;

    // 기본 무적 여부, 피격판정 적용
    public bool immortal;

    // 슈퍼아머 여부
    public bool superArmor;

    // 코루틴 동작주기
    public float waitTime;

    // 기본 이동 속도
    public List<float> speeds;

    // 사용 가능한 MoveType 리스트
    public List<MoveType> moveTypes;

    // 같은 MoveType끼리 통과가 가능한지
    public bool isPassable;

    // 효과가 적용되지 않는 상태이상
    public List<EffectType> immune;

    // 인벤토리 칸 수
    public int maxInventory;
}
