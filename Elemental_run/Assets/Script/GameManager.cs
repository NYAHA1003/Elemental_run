using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager Instance;

    public int playerGold;
    public int maxScore;
    public float playerSpeed;
    public int plusScore;
    public float playerGauge;
    public int score;

    [SerializeField]
    private GameObject tutorialBody;
    [SerializeField]
    private Image[] tutorial;
    private int tutorialCnt = -1;
    public static bool isTutorial = false;

    private void Awake()
    {
        if(null == Instance)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
        string path = Path.Combine(Application.dataPath, "database.json");

        if (isTutorial == false) tutorialBody.gameObject.SetActive(true);
    }
    public static GameManager instance
    {
        get
        {
            if(null == Instance)
            {
                return null;
            }
            return Instance;
        }
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0) && isTutorial == false)
        {
            tutorialCnt++;
            tutorial[tutorialCnt].gameObject.SetActive(false);
            if (tutorialCnt >= 3) isTutorial = true;
        }
    }
}

