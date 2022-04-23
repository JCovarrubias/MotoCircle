using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject pointPrefab;

    private GameManager gameManager;
    private ItemPool itemPool;

    private List<SpawnPoint> outsidePoints;
    private List<SpawnPoint> insidePoints;

    private float rightItemProb;
    private int itemsLimit;
    private float[] radios;
    private int[] pointsCount;

    private void Start()
    {
        gameManager = GameManager.Instance;
        itemPool = ItemPool.Instance;
        ScoreManager.Instance.m_OnAddScore.AddListener(AdjustDifficulty);
        gameManager.m_OnStart.AddListener(() =>SpawnItems(itemsLimit));
        gameManager.m_OnRestart.AddListener(InitValues);

        InitValues();
        CreateSpawnPoints();
    }

    private void InitValues()
    {
        radios = new float[] { 2.1f, 1.45f };
        pointsCount = new int[] { 12, 12 };
        rightItemProb = 70;
        itemsLimit = 5;
    }

    private void CreateSpawnPoints()
    {
        outsidePoints = new List<SpawnPoint>();
        insidePoints = new List<SpawnPoint>();

        for (int sideIndex = 0; sideIndex < radios.Length; sideIndex++)
        {
            float radio = radios[sideIndex];
            float angle = 360 / pointsCount[sideIndex];
            SpawnPointType type = sideIndex == 0 ? SpawnPointType.OUTSIDE : SpawnPointType.INSIDE;
            for (int index = 0; index < pointsCount[sideIndex]; index++)
            {
                float currentAngel = 0 + (angle * index);
                float x = Mathf.Cos(currentAngel * Mathf.Deg2Rad) * radio;
                float y = Mathf.Sin(currentAngel * Mathf.Deg2Rad) * radio;

                GameObject point = Instantiate(pointPrefab, gameObject.transform);
                point.transform.localPosition = new Vector2(y, x);
                point.transform.eulerAngles = new Vector3(0f, 0f, -currentAngel + (180 * sideIndex));
                point.SetActive(true);

                SpawnPoint spawnPoint = point.GetComponent<SpawnPoint>();
                spawnPoint.Type = type;

                if (type == SpawnPointType.OUTSIDE)
                    outsidePoints.Add(spawnPoint);
                else
                    insidePoints.Add(spawnPoint);
            }
        }
    }

    private void SpawnItems(int quantity)
    {
        for (int index = 0; index < quantity; index++)
        {
            ItemType itemType = Random.Range(0, 100) <= rightItemProb ? ItemType.RIGHT : ItemType.WRONG;
            SpawnPointType side = Random.Range(0, 100) <= 50 ? SpawnPointType.OUTSIDE : SpawnPointType.INSIDE;
            List<SpawnPoint> tempList = side == SpawnPointType.OUTSIDE ? outsidePoints : insidePoints;
            int spawnIndex = Random.Range(0, tempList.Count);
            while (tempList[spawnIndex].Item != null)
            {
                spawnIndex = Random.Range(0, tempList.Count);
            }

            ItemBehaviour item = itemPool.GetItem(itemType);
            tempList[spawnIndex].m_Emptyoint.RemoveAllListeners();
            tempList[spawnIndex].m_Emptyoint.AddListener(() => EmptyPoint(tempList[spawnIndex]));
            tempList[spawnIndex].Item = item;
            item.Point = tempList[spawnIndex];
            item.transform.localPosition = tempList[spawnIndex].transform.localPosition;
            item.transform.eulerAngles = new Vector3(0f, 0f, tempList[spawnIndex].transform.eulerAngles.z);
            item.gameObject.SetActive(true);
        }
    }

    private void EmptyPoint(SpawnPoint spawnPoint)
    {
        spawnPoint.Item.Point = null;
        spawnPoint.Item = null;
        SpawnItems(1);
    }

    private void AdjustDifficulty()
    {
        if (ScoreManager.Instance.Score % 3 == 0)
        {
            if (rightItemProb > 45) rightItemProb -= 5;
            //itemsLimit
        }
    }
}
