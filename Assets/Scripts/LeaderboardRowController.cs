using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LeaderboardRowController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI place;
    [SerializeField] TextMeshProUGUI userName;
    [SerializeField] TextMeshProUGUI score;

    public int Place
    {
        set { place.text = value.ToString(); }
    }

    public string UserName
    {
        set { userName.text = value; }
    }

    public int Score
    {
        set { score.text = value.ToString(); }
    }
}
