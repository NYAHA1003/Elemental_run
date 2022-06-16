using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMove : MonoBehaviour
{
    private Rigidbody rigidBody;
    public int obsNum = 0;

    public void True()
    {
        gameObject.SetActive(true);
    }

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Move();
    }

    public void Move()
    {
        rigidBody.velocity = new Vector3(0,0,-25f);

        if (transform.position.z <= -15)
        {
            gameObject.transform.position = new Vector3(transform.position.x, transform.position.y, 100);
            gameObject.SetActive(false);
            Invoke("True", 3f);
        }
    }

    public void HitPlayer(int itemNum)
    {
        if (itemNum == 0 || itemNum == obsNum)
        {
            Debug.Log("Ãæµ¹");
            gameObject.transform.position = new Vector3(transform.position.x, transform.position.y, 100);
            gameObject.SetActive(false);
            Invoke("True", 3f);
        }
        else return;
    }
}
