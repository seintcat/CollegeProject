using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �Ѱ� ī�װ��� ������ ������
[System.Serializable]
public class GalleryDataTag
{
    //���� ������
    public ContextList tagName;

    //������ ī�װ� ����
    public List<GalleryDataContent> contents;

    // �±� ������
    [SerializeField]
    Sprite _icon;
    public Sprite icon { get { return _icon; } }

    // �±� �̹���
    [SerializeField]
    Sprite _label;
    public Sprite label { get { return _label; } }
}
