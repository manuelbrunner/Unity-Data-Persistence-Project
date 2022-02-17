using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif



public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static string playerName;
    public HighScore highScore { get; set; }

    public static GameManager Instance { get { return _instance; } }
    private string filepath; 

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            filepath = Application.persistentDataPath + "/savefile.json";
            Load();
        }
    }

    [System.Serializable]
    public class HighScore
    {
        public int score = 0;
        public string name = "-";
    }

    public void Save(int score)
    {
        if (score > highScore.score)
        {
            HighScore data = new HighScore();
            data.score = score;
            data.name = playerName;
            highScore = data;
            Debug.Log($"Saving high score: {data.score} ({data.name}) to {filepath}");
            string json = JsonUtility.ToJson(data);

            File.WriteAllText(filepath, json);
        }
    }

    public void Load()
    {
        if (File.Exists(filepath))
        {
            string json = File.ReadAllText(filepath);
            highScore = JsonUtility.FromJson<HighScore>(json);
            Debug.Log($"Loaded high score: {highScore.score} ({highScore.name}) from {filepath}");
        } else
        {
            highScore = new HighScore();
        }
    }

    public void SetPlayerName(string name)
    {
        Debug.Log($"setting player name: {name}");
        playerName = name;
    }

    public void StartGame()
    {
        Debug.Log($"Starting game for {playerName}");
        SceneManager.LoadScene("main");
    }

    public void Exit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

}
