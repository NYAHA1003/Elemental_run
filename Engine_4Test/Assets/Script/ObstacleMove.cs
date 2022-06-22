using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMove : MonoBehaviour
{
    [SerializeField]
    private Material[] obsMaterial;
    private Rigidbody rigidBody;
    private int obsNum = 0;
    private float delay;
    private float moveSpd;
    private MeshRenderer meshRen;

    public void True()
    {
        gameObject.SetActive(true);
        moveSpd -= 5f;
    }

    void Start()
    {
        moveSpd = Mathf.Clamp(moveSpd, -125, -25);
        rigidBody = GetComponent<Rigidbody>();
        meshRen = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        delay = Mathf.Clamp(delay, 0, 4);
        delay = Random.Range(0f, 4f);
        Move();
    }

    public void SetProperty()
    {
        gameObject.transform.position = new Vector3(transform.position.x, transform.position.y, 100);
        GameManager.instance.score += GameManager.instance.plusScore;
        gameObject.SetActive(false);
        obsNum = Random.Range(0, 4);
        Debug.Log(obsNum);
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
        if (itemNum == 0 || itemNum == obsNum)
        {
            SetProperty();
        }
        else return;
    }

    public void ChangeColor(int obs)
    {
        Debug.Log(obs);
        meshRen.material = obsMaterial[obs];
    }
}
