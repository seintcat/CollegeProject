using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 코루틴, 스프라이트 등 화면 관련 불변하는 데이터
public static class ScreenSettings
{
    // 한개 블럭의 가로세로 픽셀 개수
    public static readonly byte blockSize = 32;

    // 애니메이션 재생 속도 기본값
    public static readonly float animationSpeed = 1f / 60;

    // 카메라 코루틴 체크속도
    public static readonly float cameraCheckingTime = 0.3f;

    // 임시
    static readonly float positionCheckTime;
}
