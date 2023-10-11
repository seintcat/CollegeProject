using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 2d 상황에서의 방향 정보에 대한 열거형
public enum Direction2D
{
    // 방향 없음이 디폴트값이고, 위 방향에서 시계방향 순으로 8방향 배치한다
    None,
    Up,
    UpRight,
    Right,
    DownRight, 
    Down,
    DownLeft,
    Left,
    UpLeft,
}
