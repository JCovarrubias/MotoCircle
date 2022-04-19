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

    public List<Messages> GeppMessages
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
        if(UserData == null)
        {
            UserData = new User()
            {
                bestScore = 0,
                lives = 3,
                name = "Player"
            };
            //LeaderboardUserPosition = 50;
        }
    }

    public void NotifyGameStart() => StartGame();

    public void SetUserData(string jsonData)
    {
        Debug.Log(jsonData);
        UserData = JsonUtility.FromJson<User>(jsonData);
        //LeaderBoard.FillUserData(UserData);
    }

    public void SetUserLeaderboardPosition(int pos)
    {
        Debug.Log("User leaderboard position "+pos);
        //LeaderboardUserPosition = pos;
    }

    public void SetLeaderboardData(string jsonData)
    {
        LeaderboardData = JsonUtility.FromJson<Leaderboard>(jsonData);
        //LeaderBoard.FillUsers();
    }

    public void SetMessages(string jsonData)
    {
        GeppMessages = JsonUtility.FromJson<List<Messages>>(jsonData);
        //GEPPMessagesWindow.Instance.FillMessage(GeppMessages[0].message);
    }

    public void SendResults()
    {
        /*GameResult gameResult = new GameResult {
            score = GameManager.Instance.Score,
            lives = GameManager.Instance.Lives,
            minutes = 1,
            seconds = 1,
        };
        string gameResultJson = JsonUtility.ToJson(gameResult);

        if(Application.platform == RuntimePlatform.WebGLPlayer)
            SendGameResult(gameResultJson.ToString());*/
    }

    public void EndGame()
    {
        float time = Time.realtimeSinceStartup;
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        TimeSpent timeSpent = new TimeSpent
        {
            minutes = minutes,
            seconds = seconds
        };

        string timeSpentJson = JsonUtility.ToJson(timeSpent);

        if(Application.platform == RuntimePlatform.WebGLPlayer)
            ExitGame(timeSpentJson.ToString());
    }

    public void GetEvent(string eventName)
    {
        GameEvent gameEvent = new GameEvent()
        {
            eventName = eventName
        };

        if(Application.platform == RuntimePlatform.WebGLPlayer)
            SendGameEvent(JsonUtility.ToJson(gameEvent));
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
    public int PlayerPosition;
    public List<LeaderboardRow> leaderboard;
}

[Serializable]
public class Messages
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
}

[Serializable]
public class GameEvent
{
    public string eventName;
}
