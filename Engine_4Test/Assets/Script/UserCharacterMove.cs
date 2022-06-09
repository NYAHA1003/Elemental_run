using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserCharacterMove : MonoBehaviour
{
    //캐릭터 회전 이동 속도 
    public float rotateMoveSpd = 100.0f;

    //캐릭터 회전 방향으로 몸을 돌리는 속도
    public float rotateBodySpd = 2.0f;

    //캐릭터 이동 속도 증가 값
    public float moveChageSpd = 0.1f;

    //현재 캐릭터 이동 백터 값 
    private Vector3 vecNowVelocity = Vector3.zero;

    //현재 캐릭터 이동 방향 벡터 
    private Vector3 vecMoveDirection = Vector3.zero;

    //CharacterController 캐싱 준비
    private CharacterController controllerCharacter = null;

    //캐릭터 CollisionFlags 초기값 설정
    private CollisionFlags collisionFlagsCharacter = CollisionFlags.None;

    //캐릭터 멈춤 변수 플래그
    private bool stopMove = false;

    [Header("애니메이션 속성")]
    public AnimationClip animationClipIdle = null;
    public AnimationClip animationClipRun = null;

    //컴포넌트도 필요합니다 
    private Animation animationPlayer = null;


    //캐릭터 상태  캐릭터 상태에 따라 animation을 표현
    public enum PlayerState { None, Idle, Run }

    [Header("캐릭터상태")]
    public PlayerState playerState = PlayerState.None;

    private Transform _transform;
    private bool _isJumping;
    private float _posY;
    private float _gravity;
    private float _jumpPower;
    private float _jumpTime;

    // Start is called before the first frame update
    void Start()
    {
        _transform = transform;
        _isJumping = false;
        _posY = transform.position.y;
        _gravity = 15f;
        _jumpPower = 9f;
        _jumpTime = 0.0f;

        //CharacterController 캐싱
        controllerCharacter = GetComponent<CharacterController>();

        //Animation component 캐싱
        animationPlayer = GetComponent<Animation>();
        //Animation Component 자동 재생 끄기
        animationPlayer.playAutomatically = false;
        //혹시나 재생중인 Animation 있다면? 멈추기
        animationPlayer.Stop();

        //초기 애니메이션을 설정 Enum
        playerState = PlayerState.Idle;

        //animation WrapMode : 재생 모드 설정 
        animationPlayer[animationClipIdle.name].wrapMode = WrapMode.Loop;
        animationPlayer[animationClipRun.name].wrapMode = WrapMode.Loop;
    }

    // Update is called once per frame
    void Update()
    {
        //캐릭터 이동 
        Move();
        // Debug.Log(getNowVelocityVal());
        //캐릭터 방향 변경 
        vecDirectionChangeBody();

        //현재 상태에 맞추어서 애니메이션을 재생시켜줍니다
        AnimationClipCtrl();

        //플레이어 상태 조건에 맞추어 애니메이션 재생 
        ckAnimationState();

        //점프
        Jump();
    }

    /// <summary>
    /// 이동함수 입니다 캐릭터
    /// </summary>
    void Move()
    {
        if (stopMove == true)
        {
            return;
        }
        Transform CameraTransform = Camera.main.transform;
        //메인 카메라가 바라보는 방향이 월드상에 어떤 방향인가.
        Vector3 forward = CameraTransform.TransformDirection(Vector3.forward);
        forward.y = 0.0f;

        //forward.z, forward.x
        Vector3 right = new Vector3(forward.z, 0.0f, -forward.x);

        //키입력 
        float horizontal = Input.GetAxis("Horizontal");

        //이동 + 이동 제한
        if (transform.position.x >= 443) transform.position += new Vector3(-0.1f, 0, 0);
        else if (transform.position.x <= 424) transform.position += new Vector3(0.1f, 0, 0);
        transform.position += new Vector3(horizontal / 10, 0, 0);
        
    }


    /// <summary>
    /// 현재 내 케릭터 이동 속도 가져오는 함  
    /// </summary>
    /// <returns>float</returns>
    float getNowVelocityVal()
    {
        //현재 캐릭터가 멈춰 있다면 
        if (controllerCharacter.velocity == Vector3.zero)
        {
            //반환 속도 값은 0
            vecNowVelocity = Vector3.zero;
        }
        else
        {

            //반환 속도 값은 현재 /
            Vector3 retVelocity = controllerCharacter.velocity;
            retVelocity.y = 0.0f;

            vecNowVelocity = Vector3.Lerp(vecNowVelocity, retVelocity, moveChageSpd * Time.fixedDeltaTime);

        }
        //거리 크기
        return vecNowVelocity.magnitude;
    }

    /// <summary>
    /// 캐릭터 몸통 벡터 방향 함수
    /// </summary>
    void vecDirectionChangeBody()
    {
        //캐릭터 이동 시
        if (getNowVelocityVal() > 0.0f)
        {
            //내 몸통  바라봐야 하는 곳은 어디?
            Vector3 newForward = controllerCharacter.velocity;
            newForward.y = 0.0f;

            //내 캐릭터 전면 설정 
            transform.forward = Vector3.Lerp(transform.forward, newForward, rotateBodySpd * Time.deltaTime);

        }
    }


    /// <summary>
    ///  애니메이션 재생시켜주는 함수
    /// </summary>
    /// <param name="clip">애니메이션클립</param>
    void playAnimationByClip(AnimationClip clip)
    {
        //캐싱 animation Component에 clip 할당
        //        animationPlayer.clip = clip;
        animationPlayer.GetClip(clip.name);
        //블랜딩
        animationPlayer.CrossFade(clip.name);
    }

    /// <summary>
    /// 현재 상태에 맞추어 애니메이션을 재생
    /// </summary>
    void AnimationClipCtrl()
    {
        switch (playerState)
        {
            case PlayerState.Idle:
                playAnimationByClip(animationClipIdle);
                break;
            case PlayerState.Run:
                playAnimationByClip(animationClipRun);
                break;
        }
    }

    /// <summary>
    ///  현재 상태를 체크해주는 함수
    /// </summary>
    void ckAnimationState()
    {
        //현재 속도 값
        float nowSpd = getNowVelocityVal();

        //현재 플레이어 상태
        switch (playerState)
        {
            case PlayerState.Idle:
                if (nowSpd > 0.0f)
                {
                    playerState = PlayerState.Run;
                }
                break;
            case PlayerState.Run:
                if (nowSpd < 0.01f)
                {
                    playerState = PlayerState.Idle;
                }
                break;
        }
    }

    /// <summary>
    /// 애니매이션 클립 재생이 끝날 때쯤 애니매이션 이벤트 함수를 호출
    /// </summary>
    /// <param name="clip">AnimationClip</param>
    /// <param name="FuncName">event function</param>
    void SetAnimationEvent(AnimationClip animationclip, string funcName)
    {
        //새로운 이벤트 선언
        AnimationEvent newAnimationEvent = new AnimationEvent();

        //해당 이벤트의 호출 함수는 funcName
        newAnimationEvent.functionName = funcName;

        newAnimationEvent.time = animationclip.length - 0.15f;

        animationclip.AddEvent(newAnimationEvent);
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !_isJumping)
        {
            _isJumping = true;
            _posY = _transform.position.y;
        }
        if (_isJumping)
        {
            float height = (_jumpTime * _jumpTime * (-_gravity) / 2) + (_jumpTime * _jumpPower);
            _transform.position = new Vector3(_transform.position.x, _posY + height, _transform.position.z);
            _jumpTime += Time.deltaTime;
            if (height < 0.0f)
            {
                _isJumping = false;
                _jumpTime = 0.0f;
                _transform.position = new Vector3(_transform.position.x, _posY, _transform.position.z);
            }
        }

    }
}
