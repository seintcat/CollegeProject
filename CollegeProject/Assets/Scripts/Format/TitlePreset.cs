using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ȭ���� �����ϱ� ���� ������
[CreateAssetMenu(fileName = "Title Presets", menuName = "menu/All Title preset list", order = 1)]
public class TitlePreset : ScriptableObject
{
    //������ ����Ʈ
    [SerializeField]
    List<TitleData> _list;
    public List<TitleData> list { get { return _list; } }
}
