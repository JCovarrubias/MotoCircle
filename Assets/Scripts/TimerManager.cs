using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timer;

    public int Minutes { get; set; }
    public int Seconds { get; set; }
    public string GameTime { get; set; }

    private float timeElapsed;

    private void Update()
    {
        if (GameManager.Instance.IsActive)
        {
            timeElapsed += Time.deltaTime;
            Minutes = Mathf.FloorToInt(timeElapsed / 60);
            Seconds = Mathf.FloorToInt(timeElapsed % 60);
            string temSeconds = Seconds < 10 ? "0" + Seconds : Seconds.ToString();
            GameTime = Minutes + ":" + temSeconds;
            timer.text = GameTime; 
        }
    }
}
