using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 갤러리 내용
public class GalleryContent : MonoBehaviour
{
    public Button button;
    public Image image;
    public Image icon;
    public int index;

    // 갤러리 내용 보기
    public void GalleryContentsClick()
    {
        Gallery.GalleryContentsClick(index);
    }
}
