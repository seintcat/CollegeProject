using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ��� ���� ���� ����Ʈ
[CreateAssetMenu(fileName = "Block List", menuName = "menu/All Blocks List", order = 1)]
public class BlockList : ScriptableObject
{
    public List<BlockData> blocks;
}
