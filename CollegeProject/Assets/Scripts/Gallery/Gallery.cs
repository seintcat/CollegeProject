using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// ����ȭ���� �����ϴ� �߻� Ŭ����
public abstract class Gallery : MonoBehaviour
{
    // �� ��Ҹ� �����ϴ� ������Ʈ
    [SerializeField]
    RectTransform tagPos, contentPos;

    // �� ��ư ������
    [SerializeField]
    GameObject tagBtn, contentBtn;

    // ���ο�� ������
    List<GameObject> contents = new List<GameObject>();
    Dictionary<int, Button> tags = new Dictionary<int, Button>();
    int tagIndex = 0;

    // ���ο�� ��ġ �׷�
    [SerializeField]
    GridLayoutGroup group;
    // ���ο�� ��ġ ����
    [SerializeField]
    int line;

    // ��� �̹����� ������ �Ⱥ��̰� ����
    public bool showLocked = true;

    //�ʱ�ȭ �Լ�, ���� ���� �ҷ�����
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
            // ������ �رݵ�����
            bool onlyoneUnlock = false;
            foreach (bool content in unlockData[i])
                if (content)
                {
                    onlyoneUnlock = true;
                    break;
                }

            // �±׹�ư ����
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

        // ���ο�� ����
        StartCoroutine(HandleLayoutGroup());

        // ���� ����
        MakeContents();
    }

    // ���ο�� ����
    public virtual IEnumerator HandleLayoutGroup()
    {
        yield return new WaitForEndOfFrame();
        float cellSize = (contentPos.rect.size.x - group.padding.left - group.padding.right - ((line - 1) * group.spacing.x)) / line;
        group.cellSize = new Vector2(cellSize, cellSize);
    }

    //������ ����
    public virtual void Exit()
    {
        EssentialManager.SceneSelect(1);
    }

    // ������ ���� ä���
    public virtual void MakeContents(int index = -1)
    {
        if (index > -1)
            tagIndex = index;

        //�±׹�ư �ʱ�ȭ
        foreach (Button btn in tags.Values)
            btn.interactable = true;

        if (tagIndex == 0 && !tags.ContainsKey(tagIndex))
            return;

        tags[tagIndex].interactable = false;

        // �����ư �ʱ�ȭ
        if (contents.Count > 0)
            foreach (GameObject obj in contents)
                obj.SetActive(false);

        // �����ư ��ġ
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

    // ������ ���� ����
    public static void GalleryContentsClick(int index)
    {
        ToolTipManager.Show(EssentialManager.gallery.list[GalleryManager.gallery.tagIndex].contents[index].toolTipIndex);
    }
}
