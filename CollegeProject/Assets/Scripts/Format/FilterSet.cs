using System.Collections.Generic;
using UnityEngine;

// 하나의 필터 데이터
[System.Serializable]
public class FilterSet
{
    // 필터 이름
    public string filterName;


    // 배경으로 보여줄 이미지
    [SerializeField]
    private List<Sprite> _backgroundImage;
    public List<Sprite> backgroundImage { get { return _backgroundImage; } }

    // 효과음 또는 배경음
    [SerializeField]
    private AudioClip _sound;
    public AudioClip sound { get { return _sound; } }

    // 이미지를 애니메이션으로 활용할 때, 필요한 스피드 값
    [SerializeField]
    private float _speed;
    public float speed { get { return _speed; } }

    // 해당 애니메이션이 반복인지 여부
    [SerializeField]
    private bool _isLoop;
    public bool isLoop { get { return _isLoop; } }

    // 배경 이미지가 화면 맞춤인지, 타일식 배치인지 여부
    [SerializeField]
    private bool _isTiled;
    public bool isTiled { get { return _isTiled; } }

    // 배경음의 음량
    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float _volume;
    public float volume { get { return _volume; } }
}

