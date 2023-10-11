using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// 도감화면을 구성하는 추상 클래스
public abstract class Gallery : MonoBehaviour
{
    // 각 요소를 생성하는 컴포넌트
    [SerializeField]
    RectTransform tagPos, contentPos;

    // 각 버튼 프리팹
    [SerializeField]
    GameObject tagBtn, contentBtn;

    // 내부요소 관리용
    List<GameObject> contents = new List<GameObject>();
    Dictionary<int, Button> tags = new Dictionary<int, Button>();
    int tagIndex = 0;

    // 내부요소 배치 그룹
    [SerializeField]
    GridLayoutGroup group;
    // 내부요소 배치 개수
    [SerializeField]
    int line;

    // 잠금 이미지를 보일지 안보이게 할지
    public bool showLocked = true;

    //초기화 함수, 도감 정보 불러오기
    public virtual void Init(RectTransform parent = null)
    {
        RectTransform rectTrans = gameObject.GetComponent<RectTransform>();
        if (parent != null)
            rectTrans.SetParent(parent);
        rectTrans.offsetMin = new Vector2(0, 0);
        rectTrans.offsetMax = new Vector2(0, 0);
        rectTrans.localScale = new Vector3(1f, 1f, 1f);

        gameObject.SetActive(true);

        GalleryData galleryData = EssentialManager.gallery;
        List<List<bool>> unlockData = GalleryManager.unlocked;
        for (int i = 0; i < galleryData.list.Count; i++)
        {
            // 갤러리 해금데이터
            bool onlyoneUnlock = false;
            foreach (bool content in unlockData[i])
                if (content)
                {
                    onlyoneUnlock = true;
                    break;
                }

            // 태그버튼 생성
            if (onlyoneUnlock)
            {
                GameObject tag = Instantiate(tagBtn);
                tag.transform.SetParent(tagPos);
                tag.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
                Button.ButtonClickedEvent _event = new Button.ButtonClickedEvent();
                GalleryContent _tag = tag.GetComponent<GalleryContent>();

                int index = i;
                _tag.button.onClick.AddListener(delegate { MakeContents(index); });
                if(tag.GetComponent<TextManager>() != null)
                    tag.GetComponent<TextManager>().ApplyText(galleryData.list[i].tagName);
                _tag.image.sprite = galleryData.list[i].label;
                if(_tag.icon != null)
                    _tag.icon.sprite = galleryData.list[i].icon;

                tags.Add(i, _tag.button);
            }
            else
            {
                GameObject tag = Instantiate(tagBtn);
                tag.transform.SetParent(tagPos);
                tag.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
                tag.GetComponent<Button>().enabled = false;
                if (tag.GetComponent<TextManager>() != null)
                    tag.GetComponentInChildren<TextMeshProUGUI>().text = "";
                tag.GetComponent<Image>().enabled = false;
                tag.GetComponent<GalleryContent>().icon.enabled = false;
            }
        }

        // 내부요소 정렬
        StartCoroutine(HandleLayoutGroup());

        // 내용 생성
        MakeContents();
    }

    // 내부요소 정렬
    public virtual IEnumerator HandleLayoutGroup()
    {
        yield return new WaitForEndOfFrame();
        float cellSize = (contentPos.rect.size.x - group.padding.left - group.padding.right - ((line - 1) * group.spacing.x)) / line;
        group.cellSize = new Vector2(cellSize, cellSize);
    }

    //갤러리 종료
    public virtual void Exit()
    {
        EssentialManager.SceneSelect(1);
    }

    // 갤러리 내용 채우기
    public virtual void MakeContents(int index = -1)
    {
        if (index > -1)
            tagIndex = index;

        //태그버튼 초기화
        foreach (Button btn in tags.Values)
            btn.interactable = true;

        if (tagIndex == 0 && !tags.ContainsKey(tagIndex))
            return;

        tags[tagIndex].interactable = false;

        // 내용버튼 초기화
        if (contents.Count > 0)
            foreach (GameObject obj in contents)
                obj.SetActive(false);

        // 내용버튼 배치
        int contentIndex = 0;
        GalleryDataTag galleryData = EssentialManager.gallery.list[tagIndex];
        foreach (GalleryDataContent content in galleryData.contents)
        {
            GameObject contentObj;
            if (contentIndex < contents.Count)
                contentObj = contents[contentIndex];
            else
            {
                contentObj = Instantiate(contentBtn);
                contentObj.transform.SetParent(contentPos);
                contentObj.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
                contents.Add(contentObj);
            }
            contentObj.SetActive(true);

            List<List<bool>> unlockData = GalleryManager.unlocked;
            GalleryContent galleryContent = contentObj.GetComponent<GalleryContent>();
            if (unlockData[tagIndex][contentIndex])
            {
                galleryContent.image.sprite = galleryData.contents[contentIndex].sprite;
                galleryContent.index = contentIndex;
            }
            else
            {
                if (showLocked)
                {
                    galleryContent.image.sprite = EssentialManager.gallery.locked;
                    galleryContent.button.interactable = false;
                }
                else
                    contentObj.SetActive(false);
            }
            contentIndex++;
        }
    }

    // 갤러리 내용 보기
    public static void GalleryContentsClick(int index)
    {
        ToolTipManager.Show(EssentialManager.gallery.list[GalleryManager.gallery.tagIndex].contents[index].toolTipIndex);
    }
}
