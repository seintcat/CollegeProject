using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 블록 정보 관련 리스트
[CreateAssetMenu(fileName = "Block List", menuName = "menu/All Blocks List", order = 1)]
public class BlockList : ScriptableObject
{
    public List<BlockData> blocks;
}
