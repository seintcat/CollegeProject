using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 각종 이동 방법에 대한 열거형
public enum MoveType
{
    None,
    Walk,
    Fly,
    TeleportGround,
    TeleportAir,
    Swim,
}
