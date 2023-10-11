using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� ������Ʈ�� �̵��� ����ϴ� ������Ʈ
public class Movement : MonoBehaviour
{
    // ���� ������Ʈ�� �ٶ󺸰� �ִ� ����
    private Direction2D _direction;
    public Direction2D direction
    {
        get { return _direction; }
    }

    // �̵� �ӵ�
    float _speed;
    public float speed
    {
        set { _speed = value; }
    }

    // ���� �̵� Ÿ��
    MoveType _moveType;
    public MoveType moveType
    {
        get { return _moveType; }
    }

    // �̵� Ÿ�� ���� ���� ��ȭ�� �̵��ӵ��� ����
    float speedNext;

    // ����� �̵� Ÿ�� 
    MoveType _moveTypeNext;
    public MoveType moveTypeNext
    {
        get { return _moveTypeNext; }
    }

    // �ִϸ��̼� ȿ�� ������Ʈ
    [SerializeField]
    private Animator animator;

    // �ִϸ��̼� ���� ������
    int animDirectionId, animMovingId, animMoveTypeId;

    // �Ͻ����� ��
    private bool _pause;
    public bool pause
    {
        set
        {
            animator.enabled = !value;
            _pause = value;
        }
    }

    //���� �̵��������� ���� ��
    public bool isMoving
    {
        get { return isTypeChanging || x != goalX || z != goalZ; }
    }

    // ��ǥ ��ǥ
    float goalX, goalY, goalZ;

    // Ʈ������ ���� ��ǥ�ʹ� �ٸ� ĭ ���� ���� ��ǥ
    float x, y, z;

    // ���� �ٸ� Ÿ�������� �̵��� �ǽ��ϴ� ������
    public bool isTypeChanging
    {
        get { return _moveType != _moveTypeNext; }
    }

    // ���� ��������Ʈ�� ǥ���ϴ� ������Ʈ
    public GameObject character;

    // ĳ���� ������Ʈ�� ��ġ�� ������
    Vector3 characterOffset, defaultOffset;

    // ���� ��ǥ�� Vector3�� ��ȯ
    // �̵����� ��� Ʈ������ ������ǥ�� �ٸ��� ����
    public Vector3 position
    {
        get { return new Vector3( (int) x, (int) y, (int) z); }
    }

    // �̵��ϴ� ��ǥ��ǥ�� Vector3�� ��ȯ
    public Vector3 goal
    {
        get { return new Vector3((int) goalX, goalY, (int) goalZ); }
    }

    // �ִϸ��̼� ���� �ð�
    public WaitForSeconds animationTime;

    // �ʱ�ȭ �Լ�
    public void Init(int _x, int _z, MoveType _moveType, float speed)
    {
        x = _x;
        z = _z;
        goalX = _x;
        goalZ = _z;
        this._moveType = _moveType;
        _moveTypeNext = _moveType;
        _speed = speed;
        _direction = Direction2D.Right;

        // �̵��Ǵ� Ÿ�Կ� ���� ��ǥ ���̰� ����
        y = ActorSettings.GetHeight(this._moveType);
        goalY = y;

        transform.position = new Vector3(x, y, z);
        defaultOffset = animator.transform.localPosition;
        
        character.transform.eulerAngles = Quaternion.Euler(ActorSettings.defaultRotation).eulerAngles;
        animationTime = new WaitForSeconds(ActorSettings.animationTime);

        animDirectionId = Animator.StringToHash("direction");
        animMovingId = Animator.StringToHash("isMoving");
        animMoveTypeId = Animator.StringToHash("moveType");

        StartCoroutine(Animation());
    }

    // �ش� ��ǥ�� �� ������Ʈ�� �����ϴ��� ����
    // �̵����ΰ��, �̵��ϴ� ������ǥ�� �̵����� ��ǥ��ǥ�� �� �� ������ǥ ���
    public bool IsHere(int _x, int _y, int _z)
    {
        Vector3 target = new Vector3(_x, _y, _z);
        Vector3 intGoal = goal;
        intGoal.y = (int)intGoal.y;

        if (target == position || target == intGoal)
            return true;
        else
            return false;
    }

    // �ٶ󺸴� ���� ���ϱ�
    public void LookAt(int _goalX, int _goalZ)
    {
        float degree = Mathf.Atan2(goal.x - _goalX, goal.z - _goalZ) * 180 / Mathf.PI;

        if ((degree >= 0 && degree < 45) || (degree < 0 && degree > -45))
            _direction = Direction2D.Down;
        else if (degree >= 45 && degree < 135)
            _direction = Direction2D.Left;
        else if ((degree >= 135 && degree <= 180) || (degree >= -180 && degree < -135))
            _direction = Direction2D.Up;
        else
            _direction = Direction2D.Right;
    }

    // ��ǥ �̵��� ��ǥ��ǥ ����
    public void Move(int _goalX, int _goalZ, float _speedNext = 0f, MoveType _moveTypeNext = MoveType.None)
    {
        LookAt(_goalX, _goalZ);

        if (_pause || isMoving) return;

        if (!Map.IsMovable(_moveType == MoveType.Fly || _moveType == MoveType.TeleportAir, _goalX, _goalZ))
        {
            goalX = (int)transform.position.x;
            goalZ = (int)transform.position.z;
            return;
        }

        goalX = _goalX;
        goalZ = _goalZ;

        if (_speedNext == 0f) speedNext = _speed;
        else speedNext = _speedNext;

        if (_moveTypeNext != MoveType.None) this._moveTypeNext = _moveTypeNext;
        goalY = ActorSettings.GetHeight(this._moveTypeNext);

        if (this._moveTypeNext == MoveType.Fly || this._moveTypeNext == MoveType.TeleportAir)
            characterOffset = defaultOffset + ActorSettings.flyingOffset;
        else
            characterOffset = defaultOffset;
    }
    // ���� ĭ �̵��� �̵����� ����
    public void Move(Direction2D direction, float _speedNext = 0f, MoveType _moveTypeNext = MoveType.None)
    {
        int _x = 0, _z = 0;
        x = (int)transform.position.x;
        z = (int)transform.position.z;

        switch (direction)
        {
            case Direction2D.Up:
                _x = (int)x;
                _z = (int)z + 1;
                break;

            case Direction2D.UpRight:
                _x = (int)x + 1;
                _z = (int)z + 1;
                break;

            case Direction2D.Right:
                _x = (int)x + 1;
                _z = (int)z;
                break;

            case Direction2D.DownRight:
                _x = (int)x + 1;
                _z = (int)z - 1;
                break;

            case Direction2D.Down:
                _x = (int)x;
                _z = (int)z - 1;
                break;

            case Direction2D.DownLeft:
                _x = (int)x - 1;
                _z = (int)z - 1;
                break;

            case Direction2D.Left:
                _x = (int)x - 1;
                _z = (int)z;
                break;

            case Direction2D.UpLeft:
                _z = (int)z;
                _x = (int)x - 1;
                break;

            default:
                Debug.Log("None");
                break;
        }

        Move(_x, _z, _speedNext, _moveTypeNext);
    }

    // ���� �̵� �޼���
    void Moving()
    {
        //Vector3.MoveToward�� ����Ͽ� �÷��̾��� �������� ��ǥ��ǥ�� �̵�
        transform.position = Vector3.MoveTowards(transform.position, goal, _speed * Time.deltaTime);

        //�ش� ������Ʈ�� ���������� �����ð����� Ʈ�������� ���� ��ǥ���� x, z ������ ��ǥ�� ���Խ�Ų��
        if (transform.position == goal)
        {
            _moveType = _moveTypeNext;
            _speed = speedNext;

            x = (int)transform.position.x;
            y = (int)transform.position.y;
            z = (int)transform.position.z;

            goalX = x;
            goalZ = z;
        }
    }

    IEnumerator Animation()
    {
        while (true)
        {
            animator.SetInteger(animDirectionId, ActorSettings.GetDirectionInt(direction));
            animator.SetBool(animMovingId, isMoving);
            animator.SetInteger(animMoveTypeId, ActorSettings.GetMoveTypeInt(moveType));

            character.transform.localPosition = Vector3.MoveTowards(character.transform.localPosition, characterOffset, Time.deltaTime * ActorSettings.offsetSpeed);
            yield return animationTime;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //���� �����̰� ���� �ʴٸ� �ٷ� ����
        if (_pause || !isMoving)
            return;

        //�����Ӹ��� �����̱�
        Moving();
    }
}
