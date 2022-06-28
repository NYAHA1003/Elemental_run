using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UiManager : MonoBehaviour
{

    public GameObject shopPanel;
    public TextMeshProUGUI spdCostText;
    public TextMeshProUGUI scoreCostText;
    public TextMeshProUGUI gaugeCostText;
    public GameObject dangerPanel;

    void Start()
    {
        
    }

    void Update()
    {
        if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            spdCostText.text = $"{(int)(GameManager.instance.playerSpeed * 10)} money";
            scoreCostText.text = $"{GameManager.instance.plusScore * 10} money";
            gaugeCostText.text = $"{(int)(1000 / GameManager.instance.playerGauge)} money";
        }
    }

    public void ShopOn()
    {
        shopPanel.SetActive(true);
    }
    public void ShopOff()
    {
        shopPanel.SetActive(false);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void GoStartScene()
    {
        SceneManager.LoadScene(0);
    }

    public void DangerOff()
    {
        dangerPanel.gameObject.SetActive(false);
    }

    public void UpgradeSpd()
    {
        if ((int)(GameManager.instance.playerSpeed * 10) < GameManager.instance.playerGold)
        {
            GameManager.instance.playerGold -= (int)(GameManager.instance.playerSpeed * 10);
            GameManager.instance.playerSpeed += 1f;
        }
        else
        {
            dangerPanel.gameObject.SetActive(true);
            Invoke("DangerOff", 1f);
        }
    }
    public void UpgradeScore()
    {
        if (GameManager.instance.plusScore * 10 < GameManager.instance.playerGold)
        {
            GameManager.instance.playerGold -= GameManager.instance.plusScore * 10;
            GameManager.instance.plusScore += 10;   
        }
        else
        {
            dangerPanel.gameObject.SetActive(true);
            Invoke("DangerOff", 1f);
        }
    }
    public void UpgradeGauge()
    {
        if ((int)(1000 / GameManager.instance.playerGauge) < GameManager.instance.playerGold)
        {
            GameManager.instance.playerGold -= (int)(1000 / GameManager.instance.playerGauge);
            GameManager.instance.playerGauge -= 0.1f;
        }
        else
        {
            dangerPanel.gameObject.SetActive(true);
            Invoke("DangerOff", 1f);
        }
    }
}
