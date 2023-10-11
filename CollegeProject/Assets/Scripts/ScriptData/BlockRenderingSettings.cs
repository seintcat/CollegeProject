using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 블록 하나를 표시하는 데 필요한 불변하는 데이터
public static class BlockRenderingSettings
{
    // voxelVerts = 하나의 블록 안에 있는 정점 위치에 대한 값
    public static readonly Vector3[] voxelVerts = new Vector3[20] {
        // 블렌딩을 지원하지 않는 블록
        new Vector3(0.0f, 0.0f, 0.0f),
        new Vector3(0.0f, 0.0f, 1.0f),
        new Vector3(1.0f, 0.0f, 1.0f),
        new Vector3(1.0f, 0.0f, 0.0f),

        // 블렌딩을 지원하는 블록, DownLeft
        new Vector3(0.0f, 0.0f, 0.0f),
        new Vector3(0.0f, 0.0f, 0.5f),
        new Vector3(0.5f, 0.0f, 0.5f),
        new Vector3(0.5f, 0.0f, 0.0f),

        // 블렌딩을 지원하는 블록, UpLeft
        new Vector3(0.0f, 0.0f, 0.5f),
        new Vector3(0.0f, 0.0f, 1.0f),
        new Vector3(0.5f, 0.0f, 1.0f),
        new Vector3(0.5f, 0.0f, 0.5f),

        // 블렌딩을 지원하는 블록, UpRight
        new Vector3(0.5f, 0.0f, 0.5f),
        new Vector3(0.5f, 0.0f, 1.0f),
        new Vector3(1.0f, 0.0f, 1.0f),
        new Vector3(1.0f, 0.0f, 0.5f),

        // 블렌딩을 지원하는 블록, DownRight
        new Vector3(0.5f, 0.0f, 0.0f),
        new Vector3(0.5f, 0.0f, 0.5f),
        new Vector3(1.0f, 0.0f, 0.5f),
        new Vector3(1.0f, 0.0f, 0.0f),
    };

    // voxelTris = voxelVerts를 활용해서 시계방향으로 점을 순회하여 면 구축
    public static readonly byte[] voxelTris = new byte[6] {
        // 0 1 2 0 2 3 >> 이 순서로 두개의 직각삼각형 면을 만들어서 사각면 구현
        0, 1, 2, 0, 2, 3,
    };

    // 텍스쳐 아틀라스에서 불러올 사각 텍스쳐의 각 정점 좌표
    public static readonly Vector2[] voxelUvs = new Vector2[4] {
        new Vector2 (0.0f, 0.0f),
        new Vector2 (0.0f, 1.0f),
        new Vector2 (1.0f, 1.0f),
        new Vector2 (1.0f, 0.0f),
    };
}
