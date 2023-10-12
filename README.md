# CollegeProject1
## Video
[![CollegeProject](https://i.ytimg.com/vi/jkRhfK9wDwg/sddefault.jpg?sqp=-oaymwEmCIAFEOAD8quKqQMa8AEB-AHUBoAC4AOKAgwIABABGDogOih_MA8=&rs=AOn4CLALcq1223MXHxJCdjYd_aAWZPsC3A)](https://www.youtube.com/watch?v=jkRhfK9wDwg)<br>

## Presentation
![0](https://github.com/seintcat/CollegeProject/assets/35403288/0349154b-7292-4678-b295-8e9023a187ce)<br>
총 4인 팀 작업<br>
<br>
![1](https://github.com/seintcat/CollegeProject/assets/35403288/1a2d3e74-61b7-4b41-a5c8-19ee107e52d6)<br>
유니티에서 텍스쳐 아틀라스를 생성하는 툴 제작<br>
1. Resources.LoadAll()로 특정 경로 내의 모든 이미지를 Texture2D 형식으로 불러오기<br>
2. 이미지 조각을 순서에 맞추어, 아틀라스의 원점이 좌하단에 위치하게 배치<br>
3. Texture2D 인스턴스명.EncodeToPNG()로 합친 이미지를 파일로 출력<br>
<br>
<img src="https://github-production-user-asset-6210df.s3.amazonaws.com/35403288/274153447-f5b22d06-17f6-40f6-9135-72dae7ca2fc1.jpg"/><br>
여러 타일을 하나로 묶어 관리하는 청크 시스템 제작<br>
<br>
<img src="https://github.com/seintcat/CollegeProject/assets/35403288/1f60be7e-94f9-4419-8b18-b3221a8c59ad"/><br>
현재 프로젝트에서는 카메라 시점과 깊이값 변경을 통해 이미지 순서 변경<br>
소팅 레이어 변경을 통해서도 같은 기능 구현 가능<br>
<br>
<img src="https://github.com/seintcat/CollegeProject/assets/35403288/61192041-653c-403e-be86-b60ca92d89c5"/><br>
<br>
<img src="https://github.com/seintcat/CollegeProject/assets/35403288/d2d74921-924c-4fad-99d4-ce09f73d9987"/><br>
<br>
<img src="https://github.com/seintcat/CollegeProject/assets/35403288/cf8ecf9d-f0c1-497f-85b7-e8f7bec80f0a"/><br>
<br>
<img src="https://github.com/seintcat/CollegeProject/assets/35403288/e1c4489c-31fc-4e05-94db-8d7dfc2b557e"/><br>
유니티에서 제공하는 JsonUtility 클래스는 특정 클래스에 대한 배열을 저장하지 못하는 이슈<br>
Wrapper<T> 제네릭 클래스를 통해 배열을 저장하는 Json 저장 기능 구현<br>
<br>
<img src="https://github.com/seintcat/CollegeProject/assets/35403288/d0d42e1a-6d90-4571-a327-7bc9309f9287"/><br>
룰타일을 사용하지 않고, 직접 정점과 면을 만드는 방식 사용<br>
vertices, triangles, uvs<br>
<br>
<img src="https://github.com/seintcat/CollegeProject/assets/35403288/49419b23-4cb4-4c2c-a385-586ce8850450"/><br>
<br>
<img src="https://github.com/seintcat/CollegeProject/assets/35403288/ba473130-6b97-4064-b7f0-4a246f477478"/><br>
<br>
<img src="https://github.com/seintcat/CollegeProject/assets/35403288/fa2cf6b6-c688-4fce-aade-9e9adfe1f015"/><br>
현재 프로젝트에서는 해당 방법의 pixel perfect 기능을 활용하여 맵 결과물의 그리드 문제를 해결했지만,<br>
기타 복셀 형식 게임, 계단 모델링 등에서도 비슷한 문제가 발생하는 유니티 자체적 문제로 확인됨(mipmap 오류)<br>
HLSL의 tex2Dgrad처럼 쉐이더 언어로 밉맵을 그라데이션으로 샘플링하는 코드를 수정해서 해당 오류를 근본적으로 해결 가능<br>
<br>
<img src="https://github.com/seintcat/CollegeProject/assets/35403288/b3825860-b0f8-4061-a923-bdf94589f9ac"/><br>
<br>
