using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ActorSettings
{
    // 날고 있는 효과를 주기 위한 높이값 오프셋
    public static readonly Vector3 flyingOffset = new Vector3(0, 0, 0.5f);
    // 캐릭터 기본 위치 오프셋
    public static readonly Vector3 defaultOffset = new Vector3(0.5f, 0, 0.5f);
    // 캐릭터 기본 회전 오프셋
    public static readonly Vector3 defaultRotation = new Vector3(90f, 0, 0f);
    // 캐릭터 오프셋 속도
    public static readonly float offsetSpeed = 3f;
    // 애니메이션 조절 시간
    public static readonly float animationTime = 0.3f;
    // 상태이상 계산 시간
    //public static readonly float effectTime = 1f;

    // switch로 각 이동 타입에 맞는 높이값 반환
    public static float GetHeight(MoveType moveType)
    {
        float y;

        switch (moveType)
        {
            case MoveType.Walk:
                y = LayerSettings.GetHeight(ObjectType.ActorLow);
                break;
            case MoveType.Fly:
                y = LayerSettings.GetHeight(ObjectType.ActorHigh);
                break;
            case MoveType.TeleportGround:
                y = LayerSettings.GetHeight(ObjectType.ActorLow) + 0.1f;
                break;
            case MoveType.TeleportAir:
                y = LayerSettings.GetHeight(ObjectType.ActorHigh) - 0.1f;
                break;
            case MoveType.Swim:
                y = LayerSettings.GetHeight(ObjectType.ActorLow) - 0.1f;
                break;
            default:
                Debug.Log("Wrong MoveType");
                y = 0f;
                break;
        }

        return y;
    }

    // Animator 컴포넌트 파라미터 반환용 메서드
    public static int GetMoveTypeInt(MoveType moveType)
    {
        switch (moveType)
        {
            case MoveType.Walk:
                return 0;
            case MoveType.Fly:
                return 1;
            case MoveType.TeleportGround:
                return 2;
            case MoveType.TeleportAir:
                return 3;
            case MoveType.Swim:
                return 4;
            default:
                Debug.Log("Wrong type");
                return -1;
        }
    }
    public static int GetDirectionInt(Direction2D direction)
    {
        switch (direction)
        {
            case Direction2D.Up:
                return 0;
            case Direction2D.Right:
                return 1;
            case Direction2D.Down:
                return 2;
            case Direction2D.Left:
                return 3;
            default:
                Debug.Log("Wrong direction");
                return -1;
        }
    }
}
