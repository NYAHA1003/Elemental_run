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

    void Start()
    {
        
    }

    void Update()
    {
        if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            spdCostText.text = $"{GameManager.instance.playerSpeed * 1000} gold";
            scoreCostText.text = $"{GameManager.instance.plusScore * 10} gold";
            gaugeCostText.text = $"{(int)(1000 / GameManager.instance.playerGauge)} gold";
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

    public void UpgradeSpd()
    {
        if (GameManager.instance.playerSpeed * 1000 > GameManager.instance.playerGold)
        {
            GameManager.instance.playerSpeed += 0.03f;
        }
        else
        {
            return;
        }
    }
    public void UpgradeScore()
    {
        if (GameManager.instance.plusScore * 10 > GameManager.instance.playerGold)
        {
            GameManager.instance.playerSpeed += 0.03f;
        }
        else
        {
            return;
        }
    }
    public void UpgradeGauge()
    {
        if ((int)(1000 / GameManager.instance.playerGauge) > GameManager.instance.playerGold)
        {
            GameManager.instance.playerSpeed += 0.03f;
        }
        else
        {
            return;
        }
    }
}
