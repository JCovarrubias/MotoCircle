using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Despawner : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collision)
    {
        ItemBehaviour item = collision.gameObject.GetComponent<ItemBehaviour>();
        if (item != null)
            item.m_OnCollision?.Invoke();
    }
}
