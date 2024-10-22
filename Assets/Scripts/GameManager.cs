using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public enum Levels
{
    NO_LEVEL = 0,
    LEVEL_1,
    LEVEL_2,
    LEVEL_3,
    LEVEL_4,
}

public class GameManager : MonoBehaviour
{
    public static GameManager manager;

    public Levels currentLevel;
    public int currentStars;

    public float health = 200;
    public float maxHealth = 200;

    public Dictionary<Levels, LevelData> levelCleared = new Dictionary<Levels, LevelData>();

    void Awake()
    {
        if (manager == null)
        {
            DontDestroyOnLoad(gameObject);
            manager = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerInfo.dat");
        PlayerData data = new PlayerData();
        data.health = health;
        data.maxHealth = maxHealth;
        data.levelCleared = levelCleared;
        data.currentLevel = currentLevel;

        bf.Serialize(file, data);

        file.Close();
    }

    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);

            PlayerData data = (PlayerData)bf.Deserialize(file);

            file.Close();

            health = data.health;
            maxHealth = data.maxHealth;
            levelCleared = data.levelCleared;
            currentLevel = data.currentLevel;

        }
    }
}

[Serializable]
public class PlayerData
{
    public float health;
    public float maxHealth;

    public Dictionary<Levels, LevelData> levelCleared;
    public Levels currentLevel;
}

[Serializable]
public class LevelData
{
    public bool cleared = false;
    public int stars = 0;
}