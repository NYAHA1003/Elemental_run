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
    public AudioClip clip;

    DataManager dataManager;

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
        SoundManager.instance.SFXPlay("Button", clip);
    }
    public void ShopOff()
    {
        shopPanel.SetActive(false);
        SoundManager.instance.SFXPlay("Button", clip);
    }

    public void StartGame()
    {
        PlayerManager.Instance.currentItem = 0;
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
        SoundManager.instance.SFXPlay("Button", clip);
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
        SoundManager.instance.SFXPlay("Button", clip);
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
        SoundManager.instance.SFXPlay("Button", clip);
    }

    public void QuitApp()
    {
        SoundManager.instance.SFXPlay("Button", clip);
        Application.Quit();
    }
}
