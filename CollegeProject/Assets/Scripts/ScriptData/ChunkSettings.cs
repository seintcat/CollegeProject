using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 청크를 생성하는 데 필요한 불변하는 데이터
public static class ChunkSettings
{
    // 하나의 청크에 블록이 몇개 있는지 정의
    public static readonly byte blockCountX = 10;
    public static readonly byte blockCountZ = 10;
}
