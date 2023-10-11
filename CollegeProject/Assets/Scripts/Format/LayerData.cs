using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 청크가 참조해야 하는 레이어의 데이터
public class LayerData
{
    // x, z좌표값에 해당되는 정보
    // location의 x, z 기본 크기는 Map 클래스의 sizeXZ와 같음
    public List<List<int>> location;

    // 각 레이어 타일셋
    public Tileset tileset;
}
