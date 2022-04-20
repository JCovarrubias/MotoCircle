using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;

public class DataManager : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void StartGame();

    [DllImport("__Internal")]
    private static extern void SendGameResult(string jsonData);

    [DllImport("__Internal")]
    private static extern void ExitGame(string jsonData);

    [DllImport("__Internal")]
    private static extern void SendGameEvent(string jsonData);

    public static DataManager Instance;

    public User UserData
    {
        private set;
        get;
    }

    public Leaderboard LeaderboardData
    {
        private set;
        get;
    }

    public Message GeppMessage
    {
        private set;
        get;
    }

    /*public int LeaderboardUserPosition
    {
        private set;
        get;
    }*/

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {

        if (Application.isEditor || Debug.isDebugBuild)
        {
            UserData = new User()
            {
                bestScore = 18,
                lives = 3,
                name = "Jorge"
            };

            LeaderboardData = new Leaderboard()
            {
                leaderboard = new List<LeaderboardRow>()
                {
                    new LeaderboardRow(){ place = 1, name="Juan", score = 20 },
                    new LeaderboardRow(){ place = 2, name="Pablo", score = 19 },
                    new LeaderboardRow(){ place = 3, name="Jorge", score = 18 },
                    new LeaderboardRow(){ place = 4, name="Erick", score = 17 },
                    new LeaderboardRow(){ place = 5, name="Laura", score = 16 },
                    new LeaderboardRow(){ place = 6, name="Sofía", score = 15 },
                    new LeaderboardRow(){ place = 7, name="Victor", score = 14 },
                    new LeaderboardRow(){ place = 8, name="Andres", score = 13 },
                    new LeaderboardRow(){ place = 9, name="Valerdi", score = 12 },
                    new LeaderboardRow(){ place = 10, name="Cobo", score = 11 },
                }
            };
        }     
    }

    public void NotifyGameStart() => StartGame();

    public void SetUserData(string jsonData)
    {
        UserData = JsonUtility.FromJson<User>(jsonData);
        //LeaderBoard.FillUserData(UserData);
    }

    public void SetLeaderboardData(string jsonData)
    {
        LeaderboardData = JsonUtility.FromJson<Leaderboard>(jsonData);
        //LeaderBoard.FillUsers();
    }

    public void SetMessages(string jsonData)
    {
        GeppMessage = JsonUtility.FromJson<Message>(jsonData);
        //GEPPMessagesWindow.Instance.FillMessage(GeppMessages[0].message);
    }

    public void SendResults()
    {
        GameResult gameResult = new GameResult
        {
            score = ScoreManager.Instance.Score,
            lives = GameManager.Instance.Lives,
            minutes = 1,
            seconds = 1,
        };
        string gameResultJson = JsonUtility.ToJson(gameResult);

        if (Application.platform == RuntimePlatform.WebGLPlayer)
            SendGameResult(gameResultJson.ToString());
    }

    public void EndGame()
    {
        float time = Time.realtimeSinceStartup;
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        TimeSpent timeSpent = new TimeSpent
        {
            minutes = minutes,
            seconds = seconds,
            destination = GeppMessage.destination != null && GameManager.Instance.Lives == 0 ? GeppMessage.destination : null,
        };

        string timeSpentJson = JsonUtility.ToJson(timeSpent);

        if(Application.platform == RuntimePlatform.WebGLPlayer)
            ExitGame(timeSpentJson.ToString());
    }
}

[Serializable]
public class User
{
    public string name;
    public int lives;
    public int bestScore;
}

[Serializable]
public class LeaderboardRow
{
    public int place;
    public string name;
    public int score;
}

[Serializable]
public class Leaderboard
{
    public List<LeaderboardRow> leaderboard;
}

[Serializable]
public class Message
{
    public string message;
    public string destination;
}

[Serializable]
public class GameResult
{
    public int score;
    public int lives;
    public int minutes;
    public int seconds;
}

[Serializable]
public class TimeSpent
{
    public int minutes;
    public int seconds;
    public string destination;
}

[Serializable]
public class GameEvent
{
    public string eventName;
}
