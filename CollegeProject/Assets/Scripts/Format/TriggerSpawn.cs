using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 아이템을 생성하기 위한 자료 클래스
[System.Serializable]
public class TriggerSpawn 
{
    // InstanceList에서 확인할 인덱스
    public int code;

    // 인스턴스 생성 좌표
    public int x, z;
}
