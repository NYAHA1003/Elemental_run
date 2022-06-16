using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    private static PlayerManager instance;
    public static PlayerManager Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<PlayerManager>();
                if (obj != null)
                {
                    instance = obj;
                }
                else
                {
                    var newSingleton = new GameObject("Singleton Class").AddComponent<PlayerManager>();
                    instance = newSingleton;
                }
            }
            return instance;
        }
        private set
        {
            instance = value;
        }
    }
    private void Awake()
    {
        var objs = FindObjectsOfType<PlayerManager>();
        if (objs.Length != 1)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    public int hp = 3;
    public int currentItem = 0;
    private float chargeGauge = 3f;

    void Start()
    {
        StartCoroutine(GetItem());
    }

    void Update()
    {

    }

    IEnumerator GetItem()
    {
        while(true)
        {
            currentItem = Mathf.Clamp(currentItem, 0, 2);
            yield return new WaitForSeconds(chargeGauge);
            currentItem++;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            hp--;
            if(hp <= 0)
            {
                SceneManager.LoadScene(2);
            }
            collision.gameObject.BroadcastMessage("HitPlayer", 0, SendMessageOptions.RequireReceiver);
        }
    }
}
