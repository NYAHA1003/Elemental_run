using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class ObstacleMove : MonoBehaviour
{
    [SerializeField]
    private Material[] obsMaterial;
    private Rigidbody rigidBody;
    private int obsNum = 0;
    private float delay;
    private float moveSpd;
    private MeshRenderer meshRen;
    public Image hitImage;
    public AudioClip exClip;
    public AudioClip hitClip;

    public void True()
    {
        gameObject.SetActive(true);
        moveSpd -= 5f;
    }

    void Start()
    {

        moveSpd = Mathf.Clamp(moveSpd, -200, -25);
        rigidBody = GetComponent<Rigidbody>();
        meshRen = GetComponent<MeshRenderer>();
        obsNum = Random.Range(0, 4);
        ChangeColor(obsNum);
        transform.DOMoveY(1.5f, 2.5f).SetLoops(-1, LoopType.Yoyo);
    }

    void Update()
    {
        delay = Mathf.Clamp(delay, 0,8);
        delay = Random.Range(0f, 7f);
        Move();
    }

    public void SetProperty()
    {
        gameObject.transform.position = new Vector3(transform.position.x, transform.position.y, 100);
        GameManager.instance.score += GameManager.instance.plusScore;
        gameObject.SetActive(false);
        obsNum = Random.Range(0, 4);
        ChangeColor(obsNum);
        Invoke("True", delay);
    }

    public void Move()
    {
        rigidBody.velocity = new Vector3(0,0,moveSpd);

        if (transform.position.z <= -15)
        {
            SetProperty();
        }
    }

    public void HitPlayer(int itemNum)
    {
        if (itemNum != 0 && itemNum == obsNum)
        {
            SoundManager.instance.SFXPlay("Explosion", exClip);
            SetProperty();
        }
        else if(itemNum == 0)
        {
            SoundManager.instance.SFXPlay("Hit", hitClip);
            hitImage.gameObject.SetActive(true);
            Invoke("HitOff", 0.2f);
            SetProperty();  
        }
        else return;
    }

    public void HitOff()
    {
        hitImage.gameObject.SetActive(false);
    }

    public void ChangeColor(int obs)
    {
        meshRen.material = obsMaterial[obs];
    }
}
