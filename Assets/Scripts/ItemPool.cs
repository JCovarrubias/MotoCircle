using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPool : MonoBehaviour
{
    [SerializeField] ItemBehaviour rightItem;
    [SerializeField] ItemBehaviour wrongItem;
    [SerializeField] GameObject itemsContainer;

    public static ItemPool Instance;

    private Dictionary<ItemType, List<ItemBehaviour>> items;

    private void Awake()
    {
        Instance = this;

        items = new Dictionary<ItemType, List<ItemBehaviour>>()
        {
            { ItemType.RIGHT, new List<ItemBehaviour>() },
            { ItemType.WRONG, new List<ItemBehaviour>() },
        };
        AddItems(ItemType.RIGHT, 3);
        AddItems(ItemType.WRONG, 3);
    }

    public ItemBehaviour GetItem(ItemType itemType)
    {
        if (items[itemType].Count <= 0)
        {
            AddItems(itemType, 1);
        }

        int itemIndex = Random.Range(0, items[itemType].Count);
        ItemBehaviour item = items[itemType][itemIndex];
        items[itemType].Remove(item);
        return item;
    }

    private void AddItems(ItemType itemType, int quantity)
    {
        for (int index = 0; index < quantity; index++)
        {
            int itemIndex = Random.Range(0, 12);
            ItemBehaviour itemPrefab = itemType == ItemType.RIGHT ? rightItem : wrongItem;

            ItemBehaviour item = Instantiate(itemPrefab, itemsContainer.transform);
            item.m_OnCollision.AddListener(() => ReturnItem(item));
            item.gameObject.SetActive(false);
            items[itemType].Add(item);
        }
    }

    private void ReturnItem(ItemBehaviour item)
    {
        if (item.Point)
        {
            item.Point.m_Emptyoint?.Invoke();
            item.Point = null;
        }
        item.gameObject.SetActive(false);
        items[item.Type].Add(item);
    }
}


