using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ����
[CreateAssetMenu(fileName = "Language", menuName = "menu/Language Data", order = 1)]
public class Language : ScriptableObject
{
    // �� ������
    [SerializeField]
    List<Sprite> _icons;
    public List<Sprite> icons
    {
        get { return _icons; }
    }

    // ��ϵ� ��Ʈ ���
    [SerializeField]
    public List<GlobalFont> _fonts;
    public List<GlobalFont> fonts
    {
        get { return _fonts; }
    }
}
