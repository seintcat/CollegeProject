using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 갤러리 내부 전체 데이터
[CreateAssetMenu(fileName = "Gallery Contents", menuName = "menu/All Gallery Contents", order = 1)]
public class GalleryData : ScriptableObject
{
    //갤러리 카테고리 내용
    [SerializeField]
    List<GalleryDataTag> _list;
    public List<GalleryDataTag> list
    {
        get { return _list; }
    }

    // 잠김 이미지
    [SerializeField]
    Sprite _locked;
    public Sprite locked { get { return _locked; } }
}
