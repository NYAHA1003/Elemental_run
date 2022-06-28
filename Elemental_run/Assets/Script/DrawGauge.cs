using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawGauge : MonoBehaviour
{
    [SerializeField]
    private Image[] currentItem;
    private float chargeGauge;
    private float currentGauge = 0f;
    public Image gaugeImage;

    void Start()
    {
        StartCoroutine(GetItem());
    }

    void Update()
    {
        GaugeDraw();


        ItemDraw();
    }
    IEnumerator GetItem()
    {
        chargeGauge = GameManager.instance.playerGauge;
        int currentitem = PlayerManager.Instance.currentItem;
        while (true)
        {
            currentitem = Mathf.Clamp(currentitem, 0, 2);
            yield return new WaitForSeconds(chargeGauge / 100);
            currentGauge += chargeGauge / 100;
            if (chargeGauge <= currentGauge && PlayerManager.Instance.currentItem <= 2)
            {
                currentGauge = 0;
                PlayerManager.Instance.currentItem++;
            }
        }
    }
    private void GaugeDraw()
    {
        gaugeImage.fillAmount = currentGauge / chargeGauge;
        switch (PlayerSkill.cnt)
        {
            case 1:
                gaugeImage.color = Color.red;
                break;
            case 2:
                gaugeImage.color = Color.green;
                break;
            case 3:
                gaugeImage.color = Color.blue;
                break;
            default:
                break;
        }
    }
    private void ItemDraw()
    {
        switch(PlayerManager.Instance.currentItem)
        {
            case 1:currentItem[0].gameObject.SetActive(true);
                currentItem[1].gameObject.SetActive(false);
                currentItem[2].gameObject.SetActive(false);
                break;
            case 2:currentItem[0].gameObject.SetActive(true);
                currentItem[1].gameObject.SetActive(true);
                currentItem[2].gameObject.SetActive(false);
                break;
            case 3:currentItem[0].gameObject.SetActive(true);
                currentItem[1].gameObject.SetActive(true);
                currentItem[2].gameObject.SetActive(true);
                break;
            default:
                break;
        }
    }
}
