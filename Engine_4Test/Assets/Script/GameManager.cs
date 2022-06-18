using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameManager : MonoBehaviour
{
    private static GameManager Instance;

    public int playerGold;
    public int maxScore;
    public float playerSpeed;
    public int plusScore;
    public float playerGauge;
    public int score;

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
}

