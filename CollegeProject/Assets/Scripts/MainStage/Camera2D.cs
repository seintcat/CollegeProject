using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera2D : MonoBehaviour
{
    // 카메라 기본 회전값
    readonly Vector3 basicRotation = new Vector3(90, 0, 0);
    // 현재 카메라 y축 회전속도, 고정 회전각
    float rotateSpeed = 0.0f, rotateAngle = 0.0f;
    // 회전중, 이동중 플래그
    bool isRotating = false, isMoving = false;
    // 이동 좌표
    Vector2 movingPoint = new Vector2(0, 0);

    // 따라갈 타겟의 위치정보
    public Transform target;
    // xz 평면으로부터의 거리
    public int height = 10;
    // 타겟을 따라갈 때의 부드러움
    public float moveDamping = 2.0f;
    // 이동 속도
    public float moveSpeed = 5.0f;
    // 코루틴 체크속도
    WaitForSeconds time;

    //카메라 청크이동 감지 코루틴
    IEnumerator checkMove;

    private void Awake()
    {
        time = new WaitForSeconds(ScreenSettings.cameraCheckingTime);
    }

    private void OnEnable()
    {
        checkMove = CheckMove();
        StartCoroutine(checkMove);
    }

    private void OnDisable()
    {
        StopCoroutine(checkMove);
    }

    // Start is called before the first frame update
    void Start()
    {
        // 해당 컴포넌트의 중복 사용을 허가하지 않음
        CheckDuplicate();
        Vector3 vector = gameObject.transform.position;
        vector.y = height;
        gameObject.transform.position = vector;
    }

    // 오브젝트를 따라가는 카메라의 기능은 Update 안에서 움직이는 오브젝트를 추적하기 때문에, 항상 LateUpdate에서 수행되어야 함.
    void LateUpdate()
    {
        Rotating();
        Moving();
        EndCheck();
    }

    // 해당 컴포넌트의 중복 사용을 허가하지 않음
    void CheckDuplicate()
    {
        var objs = FindObjectsOfType<Camera2D>();
        if (objs.Length != 1)
        {
            Debug.Log("error : you can use Camera2D only once.");
            Destroy(gameObject);
            return;
        }
    }

    // y축으로 회전하는 메서드
    public void Rotate(float _speed)
    {
        rotateSpeed = _speed;
        rotateAngle = 0.0f;
        isRotating = true;
    }

    // y축 고정회전각도로 회전하는 메서드
    public void RotateAngle(float _rotateAngle)
    {
        rotateSpeed = 0.0f;
        rotateAngle = _rotateAngle;
        isRotating = true;
    }

    // 회전 중단
    public void StopRotate()
    {
        rotateSpeed = 0.0f;
        isRotating = false;
    }

    // 실제 회전 동작 실행 
    void Rotating()
    {
        if (isRotating)
        {
            // y축 회전
            float currentRotationAngle = transform.eulerAngles.y;

            // 고정 회전각 회전
            if (rotateSpeed == 0.0f)
            {
                currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, rotateAngle, Time.deltaTime);
                transform.localEulerAngles = Quaternion.Euler(basicRotation + new Vector3(0, currentRotationAngle, 0)).eulerAngles;

                return;
            }

            currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, currentRotationAngle + rotateSpeed, Time.deltaTime);

            transform.localEulerAngles = Quaternion.Euler(basicRotation + new Vector3(0, currentRotationAngle, 0)).eulerAngles;
        }
        else if(transform.eulerAngles != basicRotation)
        {
            // 회전하지 않으므로, 기본 회전값으로 되돌리기
            Vector3 currentRotationAngle = transform.eulerAngles;
            currentRotationAngle.x = Mathf.LerpAngle(currentRotationAngle.x, basicRotation.x, Time.deltaTime);
            currentRotationAngle.y = Mathf.LerpAngle(currentRotationAngle.y, basicRotation.y, Time.deltaTime);
            currentRotationAngle.z = Mathf.LerpAngle(currentRotationAngle.z, basicRotation.z, Time.deltaTime);

            transform.localEulerAngles = Quaternion.Euler(currentRotationAngle).eulerAngles;
        }
    }

    // 따라갈 타겟 오브젝트 지정 
    public void SetTarget(Transform _transform)
    {
        target = _transform;
        Map.JumpChunkEvent((int)(_transform.position.x / ChunkSettings.blockCountX), (int)(_transform.position.z / ChunkSettings.blockCountZ));
        StopMove();
    }

    // 따라갈 타겟 해제
    public void TargetReset()
    {
        target = null;
    }

    // xz좌표로 카메라 천천히 이동
    public void MoveTo(int x, int z)
    {
        TargetReset();

        movingPoint = new Vector2(x, z);
        isMoving = true;
    }

    // 좌표 이동 중단
    public void StopMove()
    {
        movingPoint = new Vector2(0, 0);
        isMoving = false;
    }

    // 실제 이동 동작 실행 
    void Moving()
    {
        if (isMoving)
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(movingPoint.x, height, movingPoint.y), moveSpeed * Time.deltaTime);
        else if(target != null && target.position != transform.position)
        {
            Vector3 currentPosition = transform.position;
            currentPosition.x = Mathf.Lerp(currentPosition.x, target.position.x, moveDamping * Time.deltaTime);
            currentPosition.y = height;
            currentPosition.z = Mathf.Lerp(currentPosition.z, target.position.z, moveDamping * Time.deltaTime);

            transform.position = currentPosition;
        }
    }

    // xz좌표로 카메라 순간이동
    public void Teleport(int x, int z)
    {
        TargetReset();
        StopMove();

        transform.position = new Vector3(x, height, z);

        Map.JumpChunkEvent(x / ChunkSettings.blockCountX, z / ChunkSettings.blockCountZ);
    }

    // 고정각도 회전, 좌표 이동이 끝났는지 확인
    void EndCheck()
    {
        if (isRotating && rotateAngle != 0.0f && transform.localEulerAngles == Quaternion.Euler(basicRotation + new Vector3(0, rotateAngle, 0)).eulerAngles)
        {
            rotateAngle = 0.0f;
            isRotating = false;
        }

        if (isMoving && transform.position == new Vector3(movingPoint.x, height, movingPoint.y)) StopMove();
    }

    // 카메라 청크이동을 감지하는 코루틴
    IEnumerator CheckMove()
    {
        while (true)
        {
            if (!isMoving && target == null) yield return time;

            Map.MoveChunkEvent(Mathf.FloorToInt(transform.position.x) / ChunkSettings.blockCountX, Mathf.FloorToInt(transform.position.z) / ChunkSettings.blockCountZ);
            yield return time;
        }
    }
}
