using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
// 한개 블록의 실제 정보 클래스
public class BlockData
{
    // 이름
    public string name;

    // 통과 가능한지에 대한 여부
    public bool isPassable;

    // 블럭이 타일셋 블랜딩을 지원하는지에 대한 여부
    public bool isBlendBlock;
}
