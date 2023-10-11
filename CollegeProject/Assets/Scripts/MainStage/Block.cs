using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 청크가 가지고 있는 한 개 블록의 그래픽 클래스
public class Block
{
    // Vector3 정점 리스트
    public List<Vector3> vertices;
    // 삼각형 폴리곤 리스트
    public List<int> triangles;
    // 텍스쳐 매핑 리스트
    public List<Vector2> uvs;
    // 삼각형 정점입력에 필요한 인덱스 시작지점
    int verticesIndex;
    // 삼각형 정점입력에 필요한 인덱스 크기
    int verticesSize;

    // xz좌표 블록 렌더링 정보 생성
    public int MakeBlock(int x, int z, int _verticesIndex, Vector2[] blockUVs)
    {
        // 각종 정보 초기화
        vertices = new List<Vector3>();
        triangles = new List<int>();
        uvs = new List<Vector2>();

        verticesIndex = _verticesIndex;
        Vector3 blockPosition = new Vector3(x, 0, z);

        if (blockUVs.Length == 4) // 블럭이 블렌딩을 지원하지 않을 때
        {
            // 면 1 정점
            vertices.Add(blockPosition + BlockRenderingSettings.voxelVerts[0]);
            vertices.Add(blockPosition + BlockRenderingSettings.voxelVerts[1]);
            vertices.Add(blockPosition + BlockRenderingSettings.voxelVerts[2]);
            // 면 2 정점
            vertices.Add(blockPosition + BlockRenderingSettings.voxelVerts[0]);
            vertices.Add(blockPosition + BlockRenderingSettings.voxelVerts[2]);
            vertices.Add(blockPosition + BlockRenderingSettings.voxelVerts[3]);

            // 면 1 정점에 할당된 텍스쳐 좌표 (BlockRenderingSettings.voxelUvs)
            uvs.Add(blockUVs[0]);
            uvs.Add(blockUVs[1]);
            uvs.Add(blockUVs[2]);
            // 면 2 정점에 할당된 텍스쳐 좌표
            uvs.Add(blockUVs[0]);
            uvs.Add(blockUVs[2]);
            uvs.Add(blockUVs[3]);

            // 면 1
            triangles.Add(verticesIndex + 0);
            triangles.Add(verticesIndex + 1);
            triangles.Add(verticesIndex + 2);
            // 면 2
            triangles.Add(verticesIndex + 3);
            triangles.Add(verticesIndex + 4);
            triangles.Add(verticesIndex + 5);

            verticesSize = 6;
        }
        else // 블럭이 블렌딩을 지원할 때
        {
            verticesSize = 0;
            int indexTemp = 0;

            // 블록 파츠 DownLeft, UpLeft, UpRight, DownRight
            for (int i = 0; i < 4; i++)
            {
                // 면 1 정점
                vertices.Add(blockPosition + BlockRenderingSettings.voxelVerts[indexTemp + 4]);
                vertices.Add(blockPosition + BlockRenderingSettings.voxelVerts[indexTemp + 5]);
                vertices.Add(blockPosition + BlockRenderingSettings.voxelVerts[indexTemp + 6]);
                // 면 2 정점
                vertices.Add(blockPosition + BlockRenderingSettings.voxelVerts[indexTemp + 4]);
                vertices.Add(blockPosition + BlockRenderingSettings.voxelVerts[indexTemp + 6]);
                vertices.Add(blockPosition + BlockRenderingSettings.voxelVerts[indexTemp + 7]);

                // 면 1 정점에 할당된 텍스쳐 좌표 (BlockRenderingSettings.voxelUvs)
                uvs.Add(blockUVs[indexTemp]);
                uvs.Add(blockUVs[indexTemp + 1]);
                uvs.Add(blockUVs[indexTemp + 2]);
                // 면 2 정점에 할당된 텍스쳐 좌표
                uvs.Add(blockUVs[indexTemp]);
                uvs.Add(blockUVs[indexTemp + 2]);
                uvs.Add(blockUVs[indexTemp + 3]);

                // 면 1
                triangles.Add(verticesIndex + verticesSize + 0);
                triangles.Add(verticesIndex + verticesSize + 1);
                triangles.Add(verticesIndex + verticesSize + 2);
                // 면 2
                triangles.Add(verticesIndex + verticesSize + 3);
                triangles.Add(verticesIndex + verticesSize + 4);
                triangles.Add(verticesIndex + verticesSize + 5);

                verticesSize += 6;
                indexTemp += 4;
            }
        }

        verticesIndex += verticesSize;

        return verticesIndex;
    }

    public void MakeBlock()
    {
        // 각종 정보 초기화
        vertices = new List<Vector3>();
        triangles = new List<int>();
        uvs = new List<Vector2>();
        verticesIndex = -1;
        verticesSize = 0;
    }
}
