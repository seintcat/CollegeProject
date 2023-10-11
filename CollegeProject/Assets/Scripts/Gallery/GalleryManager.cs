using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// 갤러리 화면을 관리하는 컴포넌트
public class GalleryManager : MonoBehaviour
{
    //갤러리 화면을 표시하기 위한 위치
    [SerializeField]
    RectTransform galleryPos;

    //갤러리 화면 리스트
    [SerializeField]
    ObjList galleryList;

    //자기 자신 컴포넌트를 전역으로 사용
    private static GalleryManager manager;

    //현재 도감 화면
    static Gallery _gallery;
    public static Gallery gallery
    {
        get { return _gallery; }
    }

    // 사용 가능한지 여부
    public static bool usable = false;

    // 갤러리 내용 해금 정보
    static readonly string unlockFolderName = "/GalleryUnlock/";
    static readonly string unlockFileFormat = ".json";
    public static List<List<bool>> unlocked
    {
        get
        {
            List<List<bool>> unlocks = new List<List<bool>>();
            GalleryData content = EssentialManager.gallery;
            List<bool> value = new List<bool>();
            for (int i = 0; i < content.list.Count; i++)
            {
                FileInfo check = new FileInfo(Application.streamingAssetsPath + unlockFolderName + i + unlockFileFormat);
                if (check.Exists)
                {
                    // 데이터 읽기
                    bool[] raw = JsonHelper.FromJson<bool>(File.ReadAllText(Application.streamingAssetsPath + unlockFolderName + i + unlockFileFormat));
                    foreach (bool unlock in raw)
                        value.Add(unlock);

                    // 데이터가 불완전할때 대응
                    if (value.Count < content.list[i].contents.Count)
                        for (int j = value.Count; j < content.list[i].contents.Count; j++)
                            value.Add(false);
                }
                else
                    for (int j = 0; j < content.list[i].contents.Count; j++)
                        value.Add(false);

                File.WriteAllText(Application.streamingAssetsPath + unlockFolderName + i + unlockFileFormat, JsonHelper.ToJson(value.ToArray(), true));

                unlocks.Add(value);
                value = new List<bool>();
            }

            return unlocks;
        }

        set
        {
            for (int i = 0; i < value.Count; i++)
                File.WriteAllText(Application.streamingAssetsPath + unlockFolderName + i + unlockFileFormat, JsonHelper.ToJson(value[i].ToArray(), true));
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Awake()
    {
        Init();
    }

    //초기화 함수
    public void Init()
    {
        // 중복검사
        var objs = FindObjectsOfType<GalleryManager>();
        if (objs.Length != 1)
        {
            Debug.Log("error: you can use only one GalleryManager");
            Destroy(gameObject);
            return;
        }

        // 전역변수에 인스턴스 대입.
        manager = this;
        usable = true;
    }

    //타이틀화면 구성
    public static void MakeTitle()
    {
        manager.MakeTitle_();
    }
    public void MakeTitle_()
    {
        GameObject galleryObj = Instantiate(galleryList.list[EssentialManager.usingTitlePreset.galleryIndex]);
        _gallery = galleryObj.GetComponent<Gallery>();
        _gallery.Init(galleryPos);
    }

    public static void UnlockData(int tagIndex, int contentIndex)
    {
        List<List<bool>> unlockData = unlocked;
        if(unlockData.Count > tagIndex && unlockData[tagIndex].Count > contentIndex)
        {
            unlockData[tagIndex][contentIndex] = true;
            unlocked = unlockData;
        }
    }
}
