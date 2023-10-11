using System.Collections.Generic;
using UnityEngine;

// �ϳ��� ���� ������
[System.Serializable]
public class FilterSet
{
    // ���� �̸�
    public string filterName;


    // ������� ������ �̹���
    [SerializeField]
    private List<Sprite> _backgroundImage;
    public List<Sprite> backgroundImage { get { return _backgroundImage; } }

    // ȿ���� �Ǵ� �����
    [SerializeField]
    private AudioClip _sound;
    public AudioClip sound { get { return _sound; } }

    // �̹����� �ִϸ��̼����� Ȱ���� ��, �ʿ��� ���ǵ� ��
    [SerializeField]
    private float _speed;
    public float speed { get { return _speed; } }

    // �ش� �ִϸ��̼��� �ݺ����� ����
    [SerializeField]
    private bool _isLoop;
    public bool isLoop { get { return _isLoop; } }

    // ��� �̹����� ȭ�� ��������, Ÿ�Ͻ� ��ġ���� ����
    [SerializeField]
    private bool _isTiled;
    public bool isTiled { get { return _isTiled; } }

    // ������� ����
    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float _volume;
    public float volume { get { return _volume; } }
}

