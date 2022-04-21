using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject spawnerArea;
    [SerializeField] GameObject despawnerArea;

    private GameManager gameManager;
    private ItemPool itemPool;

    private float timeCounter;
    private float spawnTime;
    private float rightItemProb;
    private float itemSpeed;

    private void Start()
    {
        gameManager = GameManager.Instance;
        itemPool = ItemPool.Instance;
        ScoreManager.Instance.m_OnAddScore.AddListener(AdjustDifficulty);
        gameManager.m_OnRestart.AddListener(InitValues);

        InitValues();
    }

    private void Update()
    {
        if (gameManager.IsActive)
        {
            timeCounter += Time.deltaTime;
            if (timeCounter >= spawnTime)
            {
                timeCounter = 0f;
                SpawnItem();
            }
        }
    }

    private void SpawnItem()
    {
        ItemType itemType = Random.Range(0, 100) <= rightItemProb ? ItemType.RIGHT : ItemType.WRONG;
        ItemBehaviour item = itemPool.GetItem(itemType);
        item.transform.localPosition = new Vector3(Random.Range(-1.7f, 1.7f), spawnerArea.transform.position.y, 0f);
        item.transform.eulerAngles = new Vector3(item.transform.eulerAngles.x, item.transform.eulerAngles.y, Random.Range(-25, 25));
        item.gameObject.SetActive(true);
        item.MoveToY(despawnerArea.transform.position.y, itemSpeed);
    }

    private void InitValues()
    {
        timeCounter = 0f;
        spawnTime = 1f;
        rightItemProb = 70;
        itemSpeed = 2f;
    }

    private void AdjustDifficulty()
    {
        if (ScoreManager.Instance.Score % 3 == 0)
        {
            if (spawnTime > 0.3f) spawnTime -= 0.05f;
            if (rightItemProb > 45) rightItemProb -= 5;
            if (itemSpeed > 1.5f) itemSpeed -= 0.1f;
        }
    }
}
