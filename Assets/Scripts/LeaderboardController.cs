using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardController : MonoBehaviour
{
    [SerializeField] LeaderboardRowController rowPrefab;
    [SerializeField] LeaderboardRowController playerRowPrefab;
    [SerializeField] GameObject container;

    private void Awake()
    {
        DataManager.Instance.m_OnFillLeaderboard.AddListener(FillLeaderboard);
    }

    private void ResetContainer()
    {
        if (container.transform.childCount > 0)
            foreach (Transform child in container.transform)
            {
                Destroy(child.gameObject);
            }
    }

    private void FillLeaderboard()
    {
        ResetContainer();

        foreach (LeaderboardRow rowData in DataManager.Instance.LeaderboardData.leaderboard)
        {
            LeaderboardRowController prefab = DataManager.Instance.UserData.name == rowData.name ? playerRowPrefab : rowPrefab;
            LeaderboardRowController row = Instantiate(prefab, container.transform);
            row.Place = rowData.place;
            row.UserName = rowData.name;
            row.Score = rowData.score;
        }

        RectTransform contRect = container.GetComponent<RectTransform>();
        RectTransform childRect = container.transform.GetChild(0).GetComponent<RectTransform>();
        contRect.sizeDelta = new Vector2(contRect.sizeDelta.x, childRect.sizeDelta.y * DataManager.Instance.LeaderboardData.leaderboard.Count);
    }
}
