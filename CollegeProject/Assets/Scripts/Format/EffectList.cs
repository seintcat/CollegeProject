using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �����̻� ���� ������ ��ü�� ����
[CreateAssetMenu(fileName = "Effect List", menuName = "menu/Effect List", order = 1)]
public class EffectList : ScriptableObject
{
    //ȿ���� ������ ���
    //����Ʈ�� EffectType�� 1�� 1 ��Ī
    public List<EffectData> effects;

    // �� Ÿ�Կ� �ش��ϴ� ����Ʈ �ε��� ��ȯ
    public static int GetTypeIndex(EffectType type)
    {
        switch (type)
        {
            case EffectType.Burn:
                return 0;
            default:
                Debug.Log("Wrong effect type");
                return -1;
        }
    }
}
