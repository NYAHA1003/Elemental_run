using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownObs : MonoBehaviour
{
    private Rigidbody rigidBody;
    private float delay;
    private float onOff;
    private float moveSpd;
    public Material material;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
       // material = GetComponent<Material>();
    }

    void Update()
    {
        delay = Mathf.Clamp(delay, 5, 10);
        delay = Random.Range(0f, 11f);
        if (transform.position.z <= 30) material.color = new Color(material.color.r, material.color.g, material.color.b, 255);
        else material.color = new Color(material.color.r, material.color.g, material.color.b, 0);
        Move();
    }

    public void True()
    {
        gameObject.SetActive(true);
    }

    public void SetProperty()
    {
        gameObject.transform.position = new Vector3(transform.position.x, transform.position.y, 100);
        GameManager.instance.score += GameManager.instance.plusScore;
        gameObject.SetActive(false);
        Invoke("True", delay);
    }

    public void Move()
    {
        if (transform.position.z <= -15)
        {
            SetProperty();
        }
        rigidBody.velocity = new Vector3(0, 0, -25);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.BroadcastMessage("PlayerDown");
        }
        else return;
    }
}
