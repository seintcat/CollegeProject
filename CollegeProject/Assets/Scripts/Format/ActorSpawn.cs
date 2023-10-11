using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 액터를 생성하기 위한 자료 클래스
[System.Serializable]
public class ActorSpawn 
{
    // InstanceList에서 확인할 인덱스
    public int index;

    // 인스턴스 생성 좌표
    public int x, z;

    // 처음 스폰되었을 때 이동하는 방식
    public MoveType moveType;
}
