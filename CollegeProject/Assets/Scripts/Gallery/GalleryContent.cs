using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ������ ����
public class GalleryContent : MonoBehaviour
{
    public Button button;
    public Image image;
    public Image icon;
    public int index;

    // ������ ���� ����
    public void GalleryContentsClick()
    {
        Gallery.GalleryContentsClick(index);
    }
}
