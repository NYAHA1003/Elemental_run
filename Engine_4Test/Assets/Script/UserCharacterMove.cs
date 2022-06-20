using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UserCharacterMove : MonoBehaviour
{
    //CharacterController 캐싱 준비
    private CharacterController controllerCharacter = null;

    //캐릭터 멈춤 변수 플래그
    private bool stopMove = false;

    [Header("애니메이션 속성")]
    public AnimationClip animationClipRun = null;
    public AnimationClip animationClipSkill = null;

    //컴포넌트도 필요합니다 
    private Animation animationPlayer = null;


    //캐릭터 상태  캐릭터 상태에 따라 animation을 표현
    public enum PlayerState { None, Run, Jump, Item }

    [Header("캐릭터상태")]
    public PlayerState playerState = PlayerState.None;

    public float speed;

    private Transform _transform;
    private bool _isJumping = false;
    private float _posY;
    private float _gravity = 15f;   
    private float _jumpPower = 11.5f;
    private float _jumpTime = 0.0f;
    public float jumpSpeed;
    public float jumpDuration;

    // Start is called before the first frame update
    void Start()
    {
        _transform = transform;
        _posY = transform.position.y;

        //CharacterController 캐싱
        controllerCharacter = GetComponent<CharacterController>();

        //Animation component 캐싱
        animationPlayer = GetComponent<Animation>();
        //Animation Component 자동 재생 끄기
        animationPlayer.playAutomatically = false;
        //혹시나 재생중인 Animation 있다면? 멈추기
        animationPlayer.Stop();

        //초기 애니메이션을 설정 Enum
        playerState = PlayerState.Run;

        //animation WrapMode : 재생 모드 설정 
        animationPlayer[animationClipRun.name].wrapMode = WrapMode.Loop;
        animationPlayer[animationClipSkill.name].wrapMode = WrapMode.Once;
    }

    // Update is called once per frame
    void Update()
    {
            //캐릭터 이동 
            Move();

            //현재 상태에 맞추어서 애니메이션을 재생시켜줍니다
            AnimationClipCtrl();

            //점프
            Jump();
    }

    private void OnGUI()
    {
        var labelStyle = new GUIStyle();
        labelStyle.fontSize = 50;
        labelStyle.normal.textColor = Color.black;

        GUILayout.Label("플레이어 체력 : " + PlayerManager.Instance.hp.ToString(), labelStyle);
        GUILayout.Label("보유 아이템 : " + PlayerManager.Instance.currentItem.ToString(), labelStyle);
        GUILayout.Label("현재 색 : " + UsingItemRay.rayColor, labelStyle);
        GUILayout.Label("현재 점수 : " + GameManager.instance.score, labelStyle);
        GUILayout.Label("최대 점수 : " + GameManager.instance.maxScore, labelStyle);
        GUILayout.Label("보유 돈 : " + GameManager.instance.playerGold, labelStyle);
    }

    void Move()
    {
        speed = GameManager.instance.playerSpeed;
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
        if (transform.position.x >= 442) transform.position += new Vector3(-speed, 0, 0);
        else if (transform.position.x <= 428) transform.position += new Vector3(speed, 0, 0);
        transform.position += new Vector3(horizontal * speed, 0, 0);
        transform.rotation = Quaternion.Euler(new Vector3(0, horizontal * 35, 0));
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
            case PlayerState.Run:
                playAnimationByClip(animationClipRun);
                break;
            case PlayerState.Jump:
                animationPlayer.Stop();
                break;
            case PlayerState.Item:
                playAnimationByClip(animationClipSkill);
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
            playerState = PlayerState.Jump;
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
        playerState = PlayerState.Run;
    }
}
