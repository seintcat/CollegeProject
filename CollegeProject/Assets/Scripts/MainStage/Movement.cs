using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 각종 오브젝트의 이동을 담당하는 컴포넌트
public class Movement : MonoBehaviour
{
    // 현재 오브젝트가 바라보고 있는 방향
    private Direction2D _direction;
    public Direction2D direction
    {
        get { return _direction; }
    }

    // 이동 속도
    float _speed;
    public float speed
    {
        set { _speed = value; }
    }

    // 현재 이동 타입
    MoveType _moveType;
    public MoveType moveType
    {
        get { return _moveType; }
    }

    // 이동 타입 변경 이후 변화될 이동속도를 저장
    float speedNext;

    // 변경될 이동 타입 
    MoveType _moveTypeNext;
    public MoveType moveTypeNext
    {
        get { return _moveTypeNext; }
    }

    // 애니메이션 효과 컴포넌트
    [SerializeField]
    private Animator animator;

    // 애니메이션 관련 데이터
    int animDirectionId, animMovingId, animMoveTypeId;

    // 일시정지 값
    private bool _pause;
    public bool pause
    {
        set
        {
            animator.enabled = !value;
            _pause = value;
        }
    }

    //현재 이동중인지에 대한 값
    public bool isMoving
    {
        get { return isTypeChanging || x != goalX || z != goalZ; }
    }

    // 목표 좌표
    float goalX, goalY, goalZ;

    // 트랜스폼 실제 좌표와는 다른 칸 단위 현재 좌표
    float x, y, z;

    // 현재 다른 타입으로의 이동을 실시하는 중인지
    public bool isTypeChanging
    {
        get { return _moveType != _moveTypeNext; }
    }

    // 실제 스프라이트를 표시하는 오브젝트
    public GameObject character;

    // 캐릭터 오브젝트의 위치값 오프셋
    Vector3 characterOffset, defaultOffset;

    // 현재 좌표를 Vector3로 반환
    // 이동중의 경우 트랜스폼 실제좌표와 다를수 있음
    public Vector3 position
    {
        get { return new Vector3( (int) x, (int) y, (int) z); }
    }

    // 이동하는 목표좌표를 Vector3로 반환
    public Vector3 goal
    {
        get { return new Vector3((int) goalX, goalY, (int) goalZ); }
    }

    // 애니메이션 조절 시간
    public WaitForSeconds animationTime;

    // 초기화 함수
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

        // 이동되는 타입에 대한 목표 높이값 설정
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

    // 해당 좌표에 이 오브젝트가 존재하는지 여부
    // 이동중인경우, 이동하는 현재좌표와 이동중인 목표자표를 둘 다 현재좌표 취급
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

    // 바라보는 각도 구하기
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

    // 좌표 이동시 목표좌표 설정
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
    // 단일 칸 이동시 이동방향 설정
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

    // 실제 이동 메서드
    void Moving()
    {
        //Vector3.MoveToward를 사용하여 플레이어의 포지션을 목표좌표로 이동
        transform.position = Vector3.MoveTowards(transform.position, goal, _speed * Time.deltaTime);

        //해당 오브젝트는 지속적으로 일정시간마다 트랜스폼의 실제 좌표값을 x, z 정수형 좌표에 대입시킨다
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
        //지금 움직이고 있지 않다면 바로 종료
        if (_pause || !isMoving)
            return;

        //프레임마다 움직이기
        Moving();
    }
}
