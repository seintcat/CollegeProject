#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//유니티 에디터용
using UnityEditor;
// 파일 저장용
using System.IO; 

// 에디터 사용 툴 클래스
public class AtlasPacker : EditorWindow 
{
    // 한개 블록의 가로세로 픽셀 수
    int blockSize = ScreenSettings.blockSize;
    // 한개 아틀라스의 가로세로 블록 수
    int atlasSizeInBlocks = 16;
    // 한개 아틀라스의 가로세로 픽셀 수 >> blockSize * atlasSizeInBlocks 될예정
    int atlasSize;

    // rawTextures = 아틀라스를 만들기 위해 필요한 이미지들을 일단 죄다 가져온 것
    Object[] rawTextures;
    // sortedTextures = rawTextures 중 조건이 충족되어 전체 아틀라스에 추가될 이미지만 담아두는 역할
    List<Texture2D> sortedTextures = new List<Texture2D>();
    // 미리보기 이미지, 최종 결과값
    Texture2D atlas;

    // 아틀라스 저장 경로(최상단은 Asset 폴더)
    string saveDirectory = "/Resources/Atlas/Result/";
    // 아틀라스 저장 파일명
    string saveFileName = "Packed_Atlas";
    // 아틀라스 재료 폴더명
    string loadFolderDirectory = "Atlas/Input";

    // 각종 상태 등을 표시하는 텍스트 정보
    string logs = "";

    // 유니티 에디터 메뉴 생성
    [MenuItem ("Custom Tools/2D Atlas Packer")] 

    public static void ShowWindow() {
        // 창 띄우기
        EditorWindow.GetWindow(typeof(AtlasPacker)); 
    }

    // 에디터에서 해당 스크립트를 창으로 띄울 때
    private void OnGUI() {
        CalculateAtlasSize();
        // 창 제목
        GUILayout.Label("2D Atlas Packer", EditorStyles.boldLabel);
        GUILayout.Label("TileSet Block Size = " + ScreenSettings.blockSize);

        // 각종 수정 가능 자료형 표시(수정 가능 필드)
        blockSize = EditorGUILayout.IntField("Block Size", blockSize);
        atlasSizeInBlocks = EditorGUILayout.IntField("Atlas Size (in blocks)", atlasSizeInBlocks);
        saveDirectory = EditorGUILayout.TextField("Atlas Result Directory", saveDirectory);
        saveFileName = EditorGUILayout.TextField("Result File Name", saveFileName);
        loadFolderDirectory = EditorGUILayout.TextField("Image Folder Directory", loadFolderDirectory);

        // 도움말
        if (GUILayout.Button("Atlas Packer Help"))
        {
            logs = "Atlas Packer Manual";
            logs += "\nBlock Size";
            logs += "\n >> pixels of 1 block's X or Z";
            logs += "\nAtlas Size";
            logs += "\n >> blocks of total result's X or Z";
            logs += "\nAtlas Result Directory";
            logs += "\n >> Unity project/Assets~~";
            logs += "\nResult File Name";
            logs += "\n >> result file will save as ~~~.png";
            logs += "\nImage Folder Directory";
            logs += "\n >> Unity project/Assets/Resources/~~";
            logs += "\n\n***** PLEASE make input image *****";
            logs += "\n >> Default/2D";
            logs += "\n >> Alpha is transparency";
            logs += "\n >> Read/Write enable";
            logs += "\n >> Wrap Mode = repeat";
        }

        // logs를 표시
        GUILayout.Label(logs);

        // atlas를 표시
        GUILayout.Label(atlas);

        // 텍스쳐 불러오는 버튼
        if (GUILayout.Button("Load Textures"))
        { 
            LoadTextures();
            PackAtlas();
            logs += "\nAtlas Packer: Textures loaded.";
        }

        // 이전에 만들어진 텍스처 초기화
        if (GUILayout.Button("Clear Textures"))
        {
            atlas = new Texture2D(atlasSize, atlasSize);
            logs = "Atlas Packer: Textures cleared.";
        }

        // 아틀라스를 png파일로 저장 
        if (GUILayout.Button("Save Atlas")) { 
            byte[] bytes = atlas.EncodeToPNG();
            try
            {
                // 저장경로 설정
                File.WriteAllBytes(Application.dataPath + saveDirectory + saveFileName + ".png", bytes);
                logs = "Atlas Packer: Atlas file sucessfully saved. " + bytes.Length;
            }
            catch
            {
                logs = "Atlas Packer: Couldn't save atlas to file.";
            }
        }
    }

    // 텍스쳐 로드
    void LoadTextures () {
        logs = "Loading Textures...";
        
        // 이전에 로드했던 데이터 비우기(다시 로드하기 전 이전데이터 초기화)
        sortedTextures.Clear();

        // loadFolderName 폴더에 있는 Texture2D 자료형 전부 불러오기
        rawTextures = Resources.LoadAll(loadFolderDirectory, typeof(Texture2D));

        // rawTextures로 가져온 이미지 배열 아이템마다 반복
        foreach (Object tex in rawTextures) {
            Texture2D t = (Texture2D)tex;
            // 넓이와 높이 픽셀이 같을경우 sortedTextures에 추가
            if (t.width == blockSize && t.height == blockSize)
                sortedTextures.Add(t);
            // 아니라면 오류메시지 출력
            else
                logs += "\nAsset Packer: " + tex.name + " incorrect size. Texture not loaded.";
        }

        // 텍스쳐 개수 표시
        logs += "\nAtlas Packer: " + sortedTextures.Count + " successfully loaded.";
    }

    // 아틀라스를 묶어서 결과물 생성
    void PackAtlas () {
        // 빈 픽셀 정보를 가로세로 픽셀수 맞춰서 생성
        atlas = new Texture2D(atlasSize, atlasSize); 
        Color[] pixels = new Color[atlasSize * atlasSize];

        // 아틀라스 내부 픽셀 당 반복
        for (int x = 0; x < atlasSize; x++)
            for (int y = 0; y < atlasSize; y++)
            {
                // 현재 픽셀이 블록 순서로는 몇번째인지 계산
                int currentBlockX = x / blockSize; 
                int currentBlockY = y / blockSize; 
                int index = currentBlockY * atlasSizeInBlocks + currentBlockX; 

                // 블록 기준으로 픽셀의 순서가 몇번째인지 계산
                int currentPixelX = x - (currentBlockX * blockSize);
                int currentPixelY = y - (currentBlockY * blockSize);

                // 픽셀 정보에 sortedTextures의 이미지를 더 담을 수 있다면 이미지 담기
                if (index < sortedTextures.Count)
                    // 텍스쳐는 기본적으로 좌하단 > 우상단 순서로 순회
                    pixels[y * atlasSize + x] = sortedTextures[index].GetPixel(x, y);
                //아니면 투명하게 
                else
                    pixels[y * atlasSize + x] = new Color(0f, 0f, 0f, 0f);
            }

        // 텍스쳐 이미지에 픽셀값을 등록, 적용
        atlas.SetPixels(pixels);
        atlas.Apply();
    }

    void CalculateAtlasSize()
    {
        // atlasSize 계산
        atlasSize = blockSize * atlasSizeInBlocks;

        // rawTextures의 배열 크기는 atlasSize와 같아야 함
        rawTextures = new Object[atlasSize];
    }
}
#endif