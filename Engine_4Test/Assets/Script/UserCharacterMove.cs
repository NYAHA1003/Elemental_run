using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UserCharacterMove : MonoBehaviour
{
    //ĳ���� ȸ�� �̵� �ӵ� 
    public float rotateMoveSpd = 100.0f;

    //ĳ���� ȸ�� �������� ���� ������ �ӵ�
    public float rotateBodySpd = 2.0f;

    //ĳ���� �̵� �ӵ� ���� ��
    public float moveChageSpd = 0.1f;

    //���� ĳ���� �̵� ���� �� 
    private Vector3 vecNowVelocity = Vector3.zero;

    //���� ĳ���� �̵� ���� ���� 
    private Vector3 vecMoveDirection = Vector3.zero;

    //CharacterController ĳ�� �غ�
    private CharacterController controllerCharacter = null;
    
    //Rigidbody ĳ�� �غ�
    private Rigidbody rigidBody = null;

    //ĳ���� CollisionFlags �ʱⰪ ����
    private CollisionFlags collisionFlagsCharacter = CollisionFlags.None;

    //ĳ���� ���� ���� �÷���
    private bool stopMove = false;

    [Header("�ִϸ��̼� �Ӽ�")]
    public AnimationClip animationClipRun = null;
    public AnimationClip animationClipSkill = null;

    //������Ʈ�� �ʿ��մϴ� 
    private Animation animationPlayer = null;


    //ĳ���� ����  ĳ���� ���¿� ���� animation�� ǥ��
    public enum PlayerState { None, Run, Jump, Item }

    [Header("ĳ���ͻ���")]
    public PlayerState playerState = PlayerState.None;

    private Transform _transform;
    private bool _isJumping = false;
    private float _posY;
    private float _gravity = 15f;   
    private float _jumpPower = 9f;
    private float _jumpTime = 0.0f;
    public float jumpSpeed;
    public float jumpDuration;

    // Start is called before the first frame update
    void Start()
    {
        _transform = transform;
        _posY = transform.position.y;

        //CharacterController ĳ��
        controllerCharacter = GetComponent<CharacterController>();

        //Animation component ĳ��
        animationPlayer = GetComponent<Animation>();
        //Animation Component �ڵ� ��� ����
        animationPlayer.playAutomatically = false;
        //Ȥ�ó� ������� Animation �ִٸ�? ���߱�
        animationPlayer.Stop();

        //�ʱ� �ִϸ��̼��� ���� Enum
        playerState = PlayerState.Run;

        //animation WrapMode : ��� ��� ���� 
        animationPlayer[animationClipRun.name].wrapMode = WrapMode.Loop;
        animationPlayer[animationClipSkill.name].wrapMode = WrapMode.Once;
    }

    // Update is called once per frame
    void Update()
    {
            //ĳ���� �̵� 
            Move();

            //���� ���¿� ���߾ �ִϸ��̼��� ��������ݴϴ�
            AnimationClipCtrl();

            //����
            Jump();
    }

    private void OnGUI()
    {
        var labelStyle = new GUIStyle();
        labelStyle.fontSize = 50;
        labelStyle.normal.textColor = Color.black;

        GUILayout.Label("�÷��̾� ü�� : " + PlayerManager.Instance.hp.ToString(), labelStyle);

        //���� ĳ���� ���� + ũ��
        GUILayout.Label("���� ������ : " + PlayerManager.Instance.currentItem.ToString(), labelStyle);

        GUILayout.Label("���� �� : " + UsingItemRay.rayColor, labelStyle);
    }

    void Move()
    {
        if (stopMove == true)
        {
            return;
        }
        Transform CameraTransform = Camera.main.transform;
        //���� ī�޶� �ٶ󺸴� ������ ����� � �����ΰ�.
        Vector3 forward = CameraTransform.TransformDirection(Vector3.forward);
        forward.y = 0.0f;

        //forward.z, forward.x
        Vector3 right = new Vector3(forward.z, 0.0f, -forward.x);

        //Ű�Է� 
        float horizontal = Input.GetAxis("Horizontal");

        //�̵� + �̵� ����
        if (transform.position.x >= 442) transform.position += new Vector3(-0.1f, 0, 0);
        else if (transform.position.x <= 428) transform.position += new Vector3(0.1f, 0, 0);
        transform.position += new Vector3(horizontal / 10, 0, 0);
    }


    /// <summary>
    /// ���� �� �ɸ��� �̵� �ӵ� �������� ��  
    /// </summary>
    /// <returns>float</returns>
    float getNowVelocityVal()
    {
        //���� ĳ���Ͱ� ���� �ִٸ� 
        if (controllerCharacter.velocity == Vector3.zero)
        {
            //��ȯ �ӵ� ���� 0
            vecNowVelocity = Vector3.zero;
        }
        else
        {

            //��ȯ �ӵ� ���� ���� /
            Vector3 retVelocity = controllerCharacter.velocity;
            retVelocity.y = 0.0f;

            vecNowVelocity = Vector3.Lerp(vecNowVelocity, retVelocity, moveChageSpd * Time.fixedDeltaTime);

        }
        //�Ÿ� ũ��
        return vecNowVelocity.magnitude;
    }


    /// <summary>
    ///  �ִϸ��̼� ��������ִ� �Լ�
    /// </summary>
    /// <param name="clip">�ִϸ��̼�Ŭ��</param>
    void playAnimationByClip(AnimationClip clip)
    {
        //ĳ�� animation Component�� clip �Ҵ�
        //        animationPlayer.clip = clip;
        animationPlayer.GetClip(clip.name);
        //����
        animationPlayer.CrossFade(clip.name);
    }

    /// <summary>
    /// ���� ���¿� ���߾� �ִϸ��̼��� ���
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
    /// �ִϸ��̼� Ŭ�� ����� ���� ���� �ִϸ��̼� �̺�Ʈ �Լ��� ȣ��
    /// </summary>
    /// <param name="clip">AnimationClip</param>
    /// <param name="FuncName">event function</param>
    void SetAnimationEvent(AnimationClip animationclip, string funcName)
    {
        //���ο� �̺�Ʈ ����
        AnimationEvent newAnimationEvent = new AnimationEvent();

        //�ش� �̺�Ʈ�� ȣ�� �Լ��� funcName
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
