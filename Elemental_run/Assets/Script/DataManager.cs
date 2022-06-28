using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataManager : MonoBehaviour
{
    string path;

    void Start()
    {   
        path = Path.Combine(Application.dataPath + "/Data/", "database.json");

        StartCoroutine(SaveData());
        JsonLoad();
    }

    public void JsonLoad()
    {
        Debug.Log("load 실행 됨");
        SaveData saveData = new SaveData();

        if (!File.Exists(path))
        {
            Debug.Log("dd");   
            GameManager.instance.playerGold = 0;
            GameManager.instance.maxScore = 0;
            GameManager.instance.playerSpeed = 20f;
            GameManager.instance.plusScore = 100;
            GameManager.instance.playerGauge = 10f;
            JsonSave();
        }
        else
        {
            Debug.Log("불러오는중");
            string loadJson = File.ReadAllText(path);
            saveData = JsonUtility.FromJson<SaveData>(loadJson);
            if (saveData != null)
            {
                GameManager.instance.playerGold = saveData.gold;
                GameManager.instance.maxScore = saveData.maxS;
                GameManager.instance.playerSpeed = saveData.speed;
                GameManager.instance.plusScore = saveData.plusS;
                GameManager.instance.playerGauge = saveData.gauge;
            }
        }
    }
    public void JsonSave()
    {
        SaveData saveData = new SaveData();

        saveData.gold = GameManager.instance.playerGold;
        saveData.maxS = GameManager.instance.maxScore;
        saveData.speed = GameManager.instance.playerSpeed;
        saveData.plusS = GameManager.instance.plusScore;
        saveData.gauge = GameManager.instance.playerGauge;

        string json = JsonUtility.ToJson(saveData, true);

        File.WriteAllText(path, json);
        Debug.Log("저장 됨");
    }

    IEnumerator SaveData()
    {
        string path = Path.Combine(Application.dataPath + "/Data/", "database.json");
        while (true)
        {
            Debug.Log("저장중");
            JsonSave();
            yield return new WaitForSeconds(5f);
        }
    }
}
