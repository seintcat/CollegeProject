using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 상태이상 관련 데이터 전체를 저장
[CreateAssetMenu(fileName = "Effect List", menuName = "menu/Effect List", order = 1)]
public class EffectList : ScriptableObject
{
    //효과를 저장한 목록
    //리스트는 EffectType과 1대 1 매칭
    public List<EffectData> effects;

    // 각 타입에 해당하는 리스트 인덱스 반환
    public static int GetTypeIndex(EffectType type)
    {
        switch (type)
        {
            case EffectType.Burn:
                return 0;
            default:
                Debug.Log("Wrong effect type");
                return -1;
        }
    }
}
