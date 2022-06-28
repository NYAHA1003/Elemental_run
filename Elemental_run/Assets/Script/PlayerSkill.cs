using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSkill : MonoBehaviour
{
    private Color rayColor;
    static public int cnt = 1;
    private RaycastHit hit;
    private float maxDistance = 20f;

    public GameObject usingRay;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            UseItem();
        }
        else if(Input.GetMouseButtonDown(1))
        {
            ChangeColor();
            Debug.Log(cnt);
        }
    }


    public void UseItem()
    {
        if (PlayerManager.Instance.currentItem >= 1)
        {
            PlayerManager.Instance.currentItem--;
            Debug.DrawRay(usingRay.transform.position, Vector3.forward * maxDistance, Color.black, 1f);
            if (Physics.Raycast(usingRay.transform.position, Vector3.forward, out hit, maxDistance))
            {
                hit.transform.gameObject.BroadcastMessage("HitPlayer", cnt, SendMessageOptions.RequireReceiver);
            }
        }
    }
    public void ChangeColor()
    {
        cnt++;
        if (cnt > 3) cnt = 1;
    }

}
