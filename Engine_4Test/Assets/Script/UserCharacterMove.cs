using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserCharacterMove : MonoBehaviour
{
    //ĳ���� ���� �̵� �ӵ� (�ȱ�)
    public float walkMoveSpd = 2.0f;

    //ĳ���� ���� �̵� �ӵ� (�޸���)
    public float runMoveSpd = 3.5f;

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

    //ĳ���� CollisionFlags �ʱⰪ ����
    private CollisionFlags collisionFlagsCharacter = CollisionFlags.None;

    //ĳ���� �߷°�
    private float gravity = 9.8f;

    //ĳ���� �߷� �ӵ� ��
    private float verticalSpd = 0f;

    //ĳ���� ���� ���� �÷���
    private bool stopMove = false;

    [Header("�ִϸ��̼� �Ӽ�")]
    public AnimationClip animationClipIdle = null;
    public AnimationClip animationClipRun = null;

    //������Ʈ�� �ʿ��մϴ� 
    private Animation animationPlayer = null;


    //ĳ���� ����  ĳ���� ���¿� ���� animation�� ǥ��
    public enum PlayerState { None, Idle, Run }

    [Header("ĳ���ͻ���")]
    public PlayerState playerState = PlayerState.None;

    // Start is called before the first frame update
    void Start()
    {
        //CharacterController ĳ��
        controllerCharacter = GetComponent<CharacterController>();

        //Animation component ĳ��
        animationPlayer = GetComponent<Animation>();
        //Animation Component �ڵ� ��� ����
        animationPlayer.playAutomatically = false;
        //Ȥ�ó� ������� Animation �ִٸ�? ���߱�
        animationPlayer.Stop();

        //�ʱ� �ִϸ��̼��� ���� Enum
        playerState = PlayerState.Idle;

        //animation WrapMode : ��� ��� ���� 
        animationPlayer[animationClipIdle.name].wrapMode = WrapMode.Loop;
        animationPlayer[animationClipRun.name].wrapMode = WrapMode.Loop;
    }

    // Update is called once per frame
    void Update()
    {
        //ĳ���� �̵� 
        Move();
        // Debug.Log(getNowVelocityVal());
        //ĳ���� ���� ���� 
        vecDirectionChangeBody();

        //���� ���¿� ���߾ �ִϸ��̼��� ��������ݴϴ�
        AnimationClipCtrl();

        //�÷��̾� ���� ���ǿ� ���߾� �ִϸ��̼� ��� 
        ckAnimationState();

        //�߷� ����
        setGravity();
    }

    /// <summary>
    /// �̵��Լ� �Դϴ� ĳ����
    /// </summary>
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

        //�ɸ��Ͱ� �̵��ϰ��� �ϴ� ���� 
        Vector3 targetDirection = horizontal * right;

        //���� �̵��ϴ� ���⿡�� ���ϴ� �������� ȸ�� 
        vecMoveDirection = Vector3.RotateTowards(vecMoveDirection, Vector3.forward, 0, 1000.0f);

        //ĳ���� �̵� �ӵ�
        float spd = runMoveSpd;

        //�߷��̵� 
        Vector3 gravity = new Vector3(0f, verticalSpd, 0f);

        // ������ �̵� ��
        Vector3 moveAmount = (transform.position * horizontal * spd * Time.deltaTime) + gravity;

        collisionFlagsCharacter = controllerCharacter.Move(moveAmount);


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
    /// ĳ���� ���� ���� ���� �Լ�
    /// </summary>
    void vecDirectionChangeBody()
    {
        //ĳ���� �̵� ��
        if (getNowVelocityVal() > 0.0f)
        {
            //�� ����  �ٶ���� �ϴ� ���� ���?
            Vector3 newForward = controllerCharacter.velocity;
            newForward.y = 0.0f;

            //�� ĳ���� ���� ���� 
            transform.forward = Vector3.Lerp(transform.forward, newForward, rotateBodySpd * Time.deltaTime);

        }
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
            case PlayerState.Idle:
                playAnimationByClip(animationClipIdle);
                break;
            case PlayerState.Run:
                playAnimationByClip(animationClipRun);
                break;
        }
    }

    /// <summary>
    ///  ���� ���¸� üũ���ִ� �Լ�
    /// </summary>
    void ckAnimationState()
    {
        //���� �ӵ� ��
        float nowSpd = getNowVelocityVal();

        //���� �÷��̾� ����
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

    /// <summary>
    ///  ĳ���� �߷� ����
    /// </summary>
    void setGravity()
    {
        if ((collisionFlagsCharacter & CollisionFlags.CollidedBelow) != 0)
        {
            verticalSpd = 0f;
        }
        else
        {
            verticalSpd -= gravity * Time.deltaTime;
        }
    }
}
