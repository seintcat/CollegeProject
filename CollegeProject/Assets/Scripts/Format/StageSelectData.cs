using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//스테이지 선택창 구현 데이터
[CreateAssetMenu(fileName = "StageSelect(Name)", menuName = "menu/One Stage Select Data", order = 1)]
public class StageSelectData : ScriptableObject
{
    //스테이지 선택창 인덱스
    [SerializeField]
    int _selectorIndex;
    public int selectorIndex { get { return _selectorIndex; } }
    //백그라운드 레이어 인덱스
    [SerializeField]
    int _backgroundIndex;
    public int backgroundIndex { get { return _backgroundIndex; } }

    //스토리 설명 지문
    [SerializeField]
    ContextList _storyText;
    public ContextList storyText { get { return _storyText; } }

    //게임 팁 지문
    [SerializeField]
    ContextList _tipText;
    public ContextList tipText { get { return _tipText; } }

    //스테이지 선택창 메인 이미지
    [SerializeField]
    Sprite _mainImage;
    public Sprite mainImage { get { return _mainImage; } }

    //규칙들 >> null 가능
    [SerializeField]
    Sprite _rule1;
    [SerializeField]
    Sprite _rule2;
    [SerializeField]
    Sprite _rule3;
    public Sprite rule1 { get { return _rule1; } }
    public Sprite rule2 { get { return _rule2; } }
    public Sprite rule3 { get { return _rule3; } }

    //진행창 배경 >> null 가능
    [SerializeField]
    Sprite _progressBackground;
    public Sprite progressBackground { get { return _progressBackground; } }
    //진행정도 포인터 >> null 가능
    [SerializeField]
    Sprite _progressPointer;
    public Sprite progressPointer { get { return _progressPointer; } }

    //포인터를 어디에 위치시킬건지 결정
    [SerializeField]
    [Range(0.0f, 1.0f)]
    float _progress;
    public float progress { get { return _progress; } }
}
