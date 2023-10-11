using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�������� ����â ���� ������
[CreateAssetMenu(fileName = "StageSelect(Name)", menuName = "menu/One Stage Select Data", order = 1)]
public class StageSelectData : ScriptableObject
{
    //�������� ����â �ε���
    [SerializeField]
    int _selectorIndex;
    public int selectorIndex { get { return _selectorIndex; } }
    //��׶��� ���̾� �ε���
    [SerializeField]
    int _backgroundIndex;
    public int backgroundIndex { get { return _backgroundIndex; } }

    //���丮 ���� ����
    [SerializeField]
    ContextList _storyText;
    public ContextList storyText { get { return _storyText; } }

    //���� �� ����
    [SerializeField]
    ContextList _tipText;
    public ContextList tipText { get { return _tipText; } }

    //�������� ����â ���� �̹���
    [SerializeField]
    Sprite _mainImage;
    public Sprite mainImage { get { return _mainImage; } }

    //��Ģ�� >> null ����
    [SerializeField]
    Sprite _rule1;
    [SerializeField]
    Sprite _rule2;
    [SerializeField]
    Sprite _rule3;
    public Sprite rule1 { get { return _rule1; } }
    public Sprite rule2 { get { return _rule2; } }
    public Sprite rule3 { get { return _rule3; } }

    //����â ��� >> null ����
    [SerializeField]
    Sprite _progressBackground;
    public Sprite progressBackground { get { return _progressBackground; } }
    //�������� ������ >> null ����
    [SerializeField]
    Sprite _progressPointer;
    public Sprite progressPointer { get { return _progressPointer; } }

    //�����͸� ��� ��ġ��ų���� ����
    [SerializeField]
    [Range(0.0f, 1.0f)]
    float _progress;
    public float progress { get { return _progress; } }
}
