using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UserCharacterMove : MonoBehaviour
{
    //CharacterController ĳ�� �غ�
    private CharacterController controllerCharacter = null;

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
        if(GameManager.isTutorial == true)
        {
            var labelStyle = new GUIStyle();
            labelStyle.fontSize = 40;
            labelStyle.normal.textColor = Color.black;

            GUILayout.Label("�÷��̾� ü�� : " + PlayerManager.Instance.hp.ToString(), labelStyle);
            GUILayout.Label("�÷��̾� �ӵ� : " + GameManager.instance.playerSpeed.ToString(), labelStyle);
            GUILayout.Label("���� ���� : " + GameManager.instance.score, labelStyle);
            GUILayout.Label("�ִ� ���� : " + GameManager.instance.maxScore, labelStyle);
            GUILayout.Label("���� �� : " + GameManager.instance.playerGold, labelStyle);
        }
    }

    void Move()
    {
        speed = GameManager.instance.playerSpeed;
        if (stopMove == true) return;
        //Ű�Է� 
        float horizontal = Input.GetAxis("Horizontal");

        transform.rotation = Quaternion.Euler(new Vector3(0, horizontal * 35, 0));
        //�̵� + �̵� ����
        if (transform.position.x >= 442) transform.position = new Vector3(441.9f, transform.position.y, transform.position.z);
        else if (transform.position.x <= 428) transform.position = new Vector3(428.1f, transform.position.y, transform.position.z);
        transform.position += new Vector3(horizontal * speed * Time.deltaTime, 0, 0);
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

    public void PlayerDown()
    {
        stopMove = true;
        Invoke("CopyDie", 2f);
    }

    public void CopyDie()
    {
        PlayerManager.Die();
        stopMove = false;
    }
}
