using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsingItemRay : MonoBehaviour
{
    private RaycastHit hit;
    private float maxDistance = 20f;
    private int cnt = 0;
    static public Color rayColor;

    void Start()
    {
        ChangeColor();
    }

    void Update()
    {
        if(Input.GetMouseButtonUp(0))
        {
            UseItem();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            ChangeColor();
        }
    }

    public void ChangeColor()
    {
        cnt++;
        if (cnt > 3) cnt = 1;
        switch(cnt)
        {
            case 1: rayColor = Color.red;
                break;
            case 2: rayColor = Color.blue;
                break;
            case 3: rayColor = Color.green;
                break;
            default:
                break;
        }
    }    

    public void UseItem()
    {
        if(PlayerManager.Instance.currentItem >= 1)
        {
            PlayerManager.Instance.currentItem--;
            Debug.DrawRay(transform.position, transform.forward * maxDistance, rayColor, 1f);
            if (Physics.Raycast(transform.position, transform.forward, out hit, maxDistance))
            {
                hit.transform.gameObject.BroadcastMessage("HitPlayer", cnt, SendMessageOptions.RequireReceiver);
            }
        }
    }
}
