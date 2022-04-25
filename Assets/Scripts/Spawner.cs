using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject pointPrefab;

    private GameManager gameManager;
    private ItemPool itemPool;
    private PlayerBehaviour playerBehaviour;

    private List<SpawnPoint> outsidePoints;
    private List<SpawnPoint> insidePoints;

    private float rightItemProb;
    private float replaceObstacleProb;
    private int itemsLimit;
    private int obstaclesCount;
    private float[] radios;
    private int[] pointsCount;

    private void Start()
    {
        gameManager = GameManager.Instance;
        itemPool = ItemPool.Instance;
        playerBehaviour = PlayerBehaviour.Instance;
        ScoreManager.Instance.m_OnAddScore.AddListener(AdjustDifficulty);
        gameManager.m_OnStart.AddListener(() => {
            SpawnItems(1, ItemType.RIGHT);
            SpawnItems(2, ItemType.WRONG);
        });
        gameManager.m_OnRestart.AddListener(InitValues);
        playerBehaviour.m_OnPlayerAtIndex.AddListener(ReplaceObstacle);
        playerBehaviour.m_OnPlayerAtIndex.AddListener(CheckCollectablesCount);

        InitValues();
        CreateSpawnPoints();
    }

    private void InitValues()
    {
        radios = new float[] { 2.1f, 1.45f };
        pointsCount = new int[] { 12, 12 };
        rightItemProb = 50;
        replaceObstacleProb = 50;

        itemsLimit = 3;
        obstaclesCount = 0;
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
                float x = Mathf.Cos(-currentAngel * Mathf.Deg2Rad) * radio;
                float y = Mathf.Sin(-currentAngel * Mathf.Deg2Rad) * radio;

                GameObject point = Instantiate(pointPrefab, gameObject.transform);
                point.transform.localPosition = new Vector2(y, x);
                point.transform.eulerAngles = new Vector3(0f, 0f, currentAngel - (180 * sideIndex));
                point.SetActive(true);

                SpawnPoint spawnPoint = point.GetComponent<SpawnPoint>();
                spawnPoint.Type = type;
                spawnPoint.Index = index;

                if (type == SpawnPointType.OUTSIDE)
                    outsidePoints.Add(spawnPoint);
                else
                    insidePoints.Add(spawnPoint);
            }
        }
    }

    private void SpawnItems(int quantity, ItemType itemType)
    {
        for (int index = 0; index < quantity; index++)
        {
            if (itemType == ItemType.None)
                itemType = (Random.Range(0, 100) <= rightItemProb || obstaclesCount > Mathf.FloorToInt(itemsLimit / 2)) ? ItemType.RIGHT : ItemType.WRONG;

            SpawnPointType side = Random.Range(0, 100) <= 50 ? SpawnPointType.OUTSIDE : SpawnPointType.INSIDE;
            List<SpawnPoint> tempList = side == SpawnPointType.OUTSIDE ? outsidePoints : insidePoints;
            int spawnIndex = Random.Range(0, tempList.Count);
            bool blocksPlayer = ValidateObstaclePosition(side, spawnIndex, itemType) || ValidatePlayerPosition(spawnIndex);

            int numTries = 0;
            while (tempList[spawnIndex]?.Item != null || blocksPlayer)
            {
                numTries += 1;
                spawnIndex = Random.Range(0, tempList.Count);
                blocksPlayer = ValidateObstaclePosition(side, spawnIndex, itemType) || ValidatePlayerPosition(spawnIndex);

                if (!gameManager.IsActive || numTries > 10)
                    break;
            }
            if (!gameManager.IsActive || numTries > 10)
                break;

            obstaclesCount = itemType == ItemType.WRONG ? obstaclesCount + 1 : obstaclesCount;
            ItemType typeToReplace = itemType == ItemType.WRONG ? ItemType.WRONG : ItemType.None;
            ItemBehaviour item = itemPool.GetItem(itemType);
            tempList[spawnIndex].Item = item;
            tempList[spawnIndex].m_Emptyoint.RemoveAllListeners();
            tempList[spawnIndex].m_Emptyoint.AddListener(() => EmptyPoint(tempList[spawnIndex], typeToReplace));
            item.Point = tempList[spawnIndex];
            item.transform.localPosition = tempList[spawnIndex].transform.localPosition;
            item.transform.eulerAngles = new Vector3(0f, 0f, tempList[spawnIndex].transform.eulerAngles.z);
            item.gameObject.SetActive(true);
        }
    }

    private bool ValidateObstaclePosition(SpawnPointType side, int index, ItemType type)
    {
        bool blockPlayer = false;
        
        if (type == ItemType.WRONG)
        {
            List<SpawnPoint> tempList = side == SpawnPointType.OUTSIDE ? insidePoints : outsidePoints;
            for (int pointIndex = index - 1; pointIndex < index + 2; pointIndex++)
            {
                int realIndex = pointIndex > tempList.Count - 1 ? pointIndex - tempList.Count : pointIndex;
                realIndex = pointIndex == -1 ? tempList.Count - 1 : realIndex;
                blockPlayer = blockPlayer == false ? tempList[realIndex]?.Item : true;
            }

        }
        return blockPlayer;
    }

    private bool ValidatePlayerPosition(int index)
    {
        bool blockPlayer = false;

        int playerOffsettedIndex = playerBehaviour.PlayerIndex + 3;
        for (int pointIndex = playerBehaviour.PlayerIndex; pointIndex <= playerOffsettedIndex; pointIndex++)
        {
            int realIndex = pointIndex > outsidePoints.Count - 1 ? pointIndex - outsidePoints.Count : pointIndex;
            blockPlayer = blockPlayer == false ? index == realIndex : true;
        }

        return blockPlayer;
    }

    private void CheckCollectablesCount()
    {
        List<SpawnPoint> collectables = new List<SpawnPoint>();
        collectables.AddRange(outsidePoints.FindAll((spawnPoint) => HasItemType(spawnPoint, ItemType.RIGHT)));
        collectables.AddRange(insidePoints.FindAll((spawnPoint) => HasItemType(spawnPoint, ItemType.RIGHT)));

        int itemsCount = obstaclesCount + collectables.Count;
        if (itemsCount < itemsLimit)
            SpawnItems(1, ItemType.RIGHT);
    }

    private void ReplaceObstacle()
    {
        List<SpawnPoint> obstacles = new List<SpawnPoint>();
        obstacles.AddRange(outsidePoints.FindAll((spawnPoint) => HasItemType(spawnPoint, ItemType.WRONG)));
        obstacles.AddRange(insidePoints.FindAll((spawnPoint) => HasItemType(spawnPoint, ItemType.WRONG)));
        bool replaceObstacle = Random.Range(0, 100) <= replaceObstacleProb;
        if (obstacles.Count > 0 && replaceObstacle)
        {
            SpawnPoint obstacelToRemove = obstacles[Random.Range(0, (obstacles.Count - 1))];
            obstacelToRemove.Item.m_OnCollision?.Invoke();
            obstaclesCount -= 1;
        }
    }

    private bool HasItemType(SpawnPoint spawnPoint, ItemType itemType)
    {
        SpawnPoint point = null;
        if (spawnPoint.Item)
            if (spawnPoint.Item.Type == itemType)
                point = spawnPoint;
        return point != null;
    }

    private void EmptyPoint(SpawnPoint spawnPoint, ItemType itemType)
    {
        spawnPoint.Item = null;
        if (gameManager.IsActive)
            SpawnItems(1, itemType);
    }

    private void AdjustDifficulty()
    {
        if (ScoreManager.Instance.Score % 10 == 0 && itemsLimit < 6f)
            itemsLimit += 1;

        if (ScoreManager.Instance.Score % 5 == 0 &&  replaceObstacleProb < 70f) replaceObstacleProb += 5;
    }
}
