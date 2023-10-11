using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 아이템을 생성하기 위한 자료 클래스
[System.Serializable]
public class ItemSpawn 
{
    // InstanceList에서 확인할 인덱스
    public int code;

    // 인스턴스 생성 좌표
    public int x, z;

    // 생성되는 아이템 수량
    public int count;
}
