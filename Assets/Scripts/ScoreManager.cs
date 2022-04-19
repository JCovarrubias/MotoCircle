using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public int Score
    {
        get; set;
    }

    private void Awake()
    {
        Instance = this;
    }
}
