using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���͸� �����ϱ� ���� �ڷ� Ŭ����
[System.Serializable]
public class ActorSpawn 
{
    // InstanceList���� Ȯ���� �ε���
    public int index;

    // �ν��Ͻ� ���� ��ǥ
    public int x, z;

    // ó�� �����Ǿ��� �� �̵��ϴ� ���
    public MoveType moveType;
}
