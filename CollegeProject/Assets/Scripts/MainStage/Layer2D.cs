using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Layer2D : MonoBehaviour
{
    // 해당 레이어가 관리해야 하는 모든 청크
    private List<List<Chunk>> chunks = new List<List<Chunk>>();
    // 인덱스 높이값
    private int index;

    // index값에 맞는 레이어 정보를 map클래스에서 불러옴 / byte index에서 이미 넘어옴
    // ChunkFactory에서 GetChunks(index)로 Chunk리스트를 받아옴
    public Layer2D Init(int _index)
    {
        index = _index;
        chunks = Map.chunkFactory.GetChunks(_index);

        // 각 레이어에 맞추어 레이어 이름을 LayerSettings의 index에 맞추어 다시 지정
        gameObject.name = LayerSettings.LayerName(_index);

        return this;
    }

    void ShiftUp()
    {
        foreach (List<Chunk> list in chunks)
        {
            list.Add(list[0]);
            list[0].MoveChunk(list[0].x, list[0].z + LayerSettings.chunkCountZ);
            list[0].BlockRend();
            list.RemoveAt(0);
        }
    }
    void ShiftRight()
    {
        chunks.Add(chunks[0]);
        foreach (Chunk item in chunks[0])
        {
            item.MoveChunk(item.x + LayerSettings.chunkCountX, item.z);
            item.BlockRend();
        }
        chunks.RemoveAt(0);
    }
    void ShiftDown()
    {
        foreach (List<Chunk> list in chunks)
        {
            list.Insert(0, list[LayerSettings.chunkCountZ - 1]);
            list[0].MoveChunk(list[0].x, list[0].z - LayerSettings.chunkCountZ);
            list[0].BlockRend();
            list.RemoveAt(LayerSettings.chunkCountZ);
        }
    }
    void ShiftLeft()
    {
        chunks.Insert(0, chunks[LayerSettings.chunkCountX - 1]);
        foreach (Chunk item in chunks[0])
        {
            item.MoveChunk(item.x - LayerSettings.chunkCountX, item.z);
            item.BlockRend();
        }
        chunks.RemoveAt(LayerSettings.chunkCountX);
    }

    // 해당 xz좌표의 청크, 링크, 방향으로 끝을 찾아 해당 라인을 완전 반대편으로 이동시킴
    public void ChunkShift(Direction2D direction, int repeat)
    {
        // 반복회수가 0 이하라면 바로 종료
        if (repeat <= 0) return;

        switch (direction)
        {// BlockRend
            case Direction2D.Up:
                for (int i = 0; i < repeat; i++) ShiftUp();
                break;
            case Direction2D.UpRight:
                for (int i = 0; i < repeat; i++)
                {
                    ShiftUp();
                    ShiftRight();
                }
                    break;
            case Direction2D.Right:
                for (int i = 0; i < repeat; i++) ShiftRight();
                break;
            case Direction2D.DownRight:
                for (int i = 0; i < repeat; i++)
                {
                    ShiftRight();
                    ShiftDown();
                }
                break;
            case Direction2D.Down:
                for (int i = 0; i < repeat; i++) ShiftDown();
                break;
            case Direction2D.DownLeft:
                for (int i = 0; i < repeat; i++)
                {
                    ShiftDown();
                    ShiftLeft();
                }
                break;
            case Direction2D.Left:
                for (int i = 0; i < repeat; i++) ShiftLeft();
                break;
            case Direction2D.UpLeft:
                for (int i = 0; i < repeat; i++)
                {
                    ShiftLeft();
                    ShiftUp();
                }
                break;
            default:
                Debug.Log("error : direction error, " + direction);
                break;
        }
    }

    // 맵을 초기에 불러올 때나, 플레이어가 텔레포트 등을 사용할 때 전체 청크를 재구성하는 이벤트
    // ChunkFactory의 GetChunks() 사용
    public void JumpChunkEvent(int centerX, int centerZ)
    {
        chunks = Map.chunkFactory.GetChunks(index);

        for (int x = 0; x < LayerSettings.chunkCountX; x++)
            for (int z = 0; z < LayerSettings.chunkCountZ; z++)
            {
                //chunks[x][z].MoveChunk(centerX - LayerSettings.halfCountX + x, centerZ - LayerSettings.halfCountZ + z);
                chunks[x][z].MoveChunk(centerX + x, centerZ + z);
                chunks[x][z].BlockRend();
            }
    }

    // Map에서 레이어 기준 x, z좌표의 블록 외형 정보를 수정한 이벤트
    public void BlockChangeEvent(int x, int z)
    {
        foreach (List<Chunk> list in chunks)
            foreach (Chunk chunk in list)
                chunk.BlockChangeEvent(x, z);
    }

    // 레이어 자료 리셋
    public void ResetLayer()
    {
        // 청크팩토리에 청크 반환
        Map.chunkFactory.ReturnChunks(index);

        index = 0;
        chunks = new List<List<Chunk>>();

        // 각 레이어에 맞추어 레이어 이름을 LayerSettings의 index에 맞추어 다시 지정
        gameObject.name = LayerSettings.LayerName(-1);
    }
}
