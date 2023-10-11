using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 맵 정보를 읽어오는 데이터
public class MapData
{
    // 맵의 크기 관련 데이터
    public short mapSizeX, mapSizeZ;

    // 플레이어 중심 청크 좌표계
    public int centerX, centerZ;

    // 맵 백그라운드 기본 컬러
    public Color backgroundColor;

    // 맵 백그라운드 기본 배경 인덱스
    public int background;
}
