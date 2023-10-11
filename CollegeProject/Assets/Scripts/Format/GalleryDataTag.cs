using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 한개 카테고리의 갤러리 데이터
[System.Serializable]
public class GalleryDataTag
{
    //지문 데이터
    public ContextList tagName;

    //갤러리 카테고리 내용
    public List<GalleryDataContent> contents;

    // 태그 아이콘
    [SerializeField]
    Sprite _icon;
    public Sprite icon { get { return _icon; } }

    // 태그 이미지
    [SerializeField]
    Sprite _label;
    public Sprite label { get { return _label; } }
}
