using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������ ���� ��ü ������
[CreateAssetMenu(fileName = "Gallery Contents", menuName = "menu/All Gallery Contents", order = 1)]
public class GalleryData : ScriptableObject
{
    //������ ī�װ� ����
    [SerializeField]
    List<GalleryDataTag> _list;
    public List<GalleryDataTag> list
    {
        get { return _list; }
    }

    // ��� �̹���
    [SerializeField]
    Sprite _locked;
    public Sprite locked { get { return _locked; } }
}
