using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera2D : MonoBehaviour
{
    // ī�޶� �⺻ ȸ����
    readonly Vector3 basicRotation = new Vector3(90, 0, 0);
    // ���� ī�޶� y�� ȸ���ӵ�, ���� ȸ����
    float rotateSpeed = 0.0f, rotateAngle = 0.0f;
    // ȸ����, �̵��� �÷���
    bool isRotating = false, isMoving = false;
    // �̵� ��ǥ
    Vector2 movingPoint = new Vector2(0, 0);

    // ���� Ÿ���� ��ġ����
    public Transform target;
    // xz ������κ����� �Ÿ�
    public int height = 10;
    // Ÿ���� ���� ���� �ε巯��
    public float moveDamping = 2.0f;
    // �̵� �ӵ�
    public float moveSpeed = 5.0f;
    // �ڷ�ƾ üũ�ӵ�
    WaitForSeconds time;

    //ī�޶� ûũ�̵� ���� �ڷ�ƾ
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
        // �ش� ������Ʈ�� �ߺ� ����� �㰡���� ����
        CheckDuplicate();
        Vector3 vector = gameObject.transform.position;
        vector.y = height;
        gameObject.transform.position = vector;
    }

    // ������Ʈ�� ���󰡴� ī�޶��� ����� Update �ȿ��� �����̴� ������Ʈ�� �����ϱ� ������, �׻� LateUpdate���� ����Ǿ�� ��.
    void LateUpdate()
    {
        Rotating();
        Moving();
        EndCheck();
    }

    // �ش� ������Ʈ�� �ߺ� ����� �㰡���� ����
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

    // y������ ȸ���ϴ� �޼���
    public void Rotate(float _speed)
    {
        rotateSpeed = _speed;
        rotateAngle = 0.0f;
        isRotating = true;
    }

    // y�� ����ȸ�������� ȸ���ϴ� �޼���
    public void RotateAngle(float _rotateAngle)
    {
        rotateSpeed = 0.0f;
        rotateAngle = _rotateAngle;
        isRotating = true;
    }

    // ȸ�� �ߴ�
    public void StopRotate()
    {
        rotateSpeed = 0.0f;
        isRotating = false;
    }

    // ���� ȸ�� ���� ���� 
    void Rotating()
    {
        if (isRotating)
        {
            // y�� ȸ��
            float currentRotationAngle = transform.eulerAngles.y;

            // ���� ȸ���� ȸ��
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
            // ȸ������ �����Ƿ�, �⺻ ȸ�������� �ǵ�����
            Vector3 currentRotationAngle = transform.eulerAngles;
            currentRotationAngle.x = Mathf.LerpAngle(currentRotationAngle.x, basicRotation.x, Time.deltaTime);
            currentRotationAngle.y = Mathf.LerpAngle(currentRotationAngle.y, basicRotation.y, Time.deltaTime);
            currentRotationAngle.z = Mathf.LerpAngle(currentRotationAngle.z, basicRotation.z, Time.deltaTime);

            transform.localEulerAngles = Quaternion.Euler(currentRotationAngle).eulerAngles;
        }
    }

    // ���� Ÿ�� ������Ʈ ���� 
    public void SetTarget(Transform _transform)
    {
        target = _transform;
        Map.JumpChunkEvent((int)(_transform.position.x / ChunkSettings.blockCountX), (int)(_transform.position.z / ChunkSettings.blockCountZ));
        StopMove();
    }

    // ���� Ÿ�� ����
    public void TargetReset()
    {
        target = null;
    }

    // xz��ǥ�� ī�޶� õõ�� �̵�
    public void MoveTo(int x, int z)
    {
        TargetReset();

        movingPoint = new Vector2(x, z);
        isMoving = true;
    }

    // ��ǥ �̵� �ߴ�
    public void StopMove()
    {
        movingPoint = new Vector2(0, 0);
        isMoving = false;
    }

    // ���� �̵� ���� ���� 
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

    // xz��ǥ�� ī�޶� �����̵�
    public void Teleport(int x, int z)
    {
        TargetReset();
        StopMove();

        transform.position = new Vector3(x, height, z);

        Map.JumpChunkEvent(x / ChunkSettings.blockCountX, z / ChunkSettings.blockCountZ);
    }

    // �������� ȸ��, ��ǥ �̵��� �������� Ȯ��
    void EndCheck()
    {
        if (isRotating && rotateAngle != 0.0f && transform.localEulerAngles == Quaternion.Euler(basicRotation + new Vector3(0, rotateAngle, 0)).eulerAngles)
        {
            rotateAngle = 0.0f;
            isRotating = false;
        }

        if (isMoving && transform.position == new Vector3(movingPoint.x, height, movingPoint.y)) StopMove();
    }

    // ī�޶� ûũ�̵��� �����ϴ� �ڷ�ƾ
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
