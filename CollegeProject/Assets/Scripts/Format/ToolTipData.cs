using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 툴팁을 띄우는 데 필요한 요소
[System.Serializable]
public class ToolTipData
{
    //적용되는 이미지 소스
    //0번 = 대화창, 1번부터 기타 이미지등
    [SerializeField]
    List<Sprite> _sprites;
    public List<Sprite> sprites { get { return _sprites; } }

    //비율 피터 비율값, 첫값은 무시됨
    [SerializeField]
    List<float> _raitios;
    public List<float> raitios { get { return _raitios; } }

    //이미지 크기 배율 설정
    [SerializeField]
    List<Vector2> _scale;
    public List<Vector2> scale { get { return _scale; } }

    //툴팁 텍스트
    [SerializeField]
    ContextList _context;
    public ContextList context { get { return _context; } }

    //재생할 필터 인덱스, -1시 사용안함
    [SerializeField]
    int _filterIndex;
    public int filterIndex { get { return _filterIndex; } }

    //자동 재생 시간, -1시 자동 종료 안됨
    [SerializeField]
    float _time;
    public float time { get { return _time; } }

    //종료시 이어서 재생될 툴팁, -1시 없음
    [SerializeField]
    int _chainIndex;
    public int chainIndex { get { return _chainIndex; } }

    //사용하는 툴팁 유형
    [SerializeField]
    int _frameIndex;
    public int frameIndex { get { return _frameIndex; } }
}
