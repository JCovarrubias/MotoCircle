using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collision)
    {
        ItemBehaviour item = collision.gameObject.GetComponent<ItemBehaviour>();
        if (item != null)
        {
            item.m_OnCollision?.Invoke();

            if (item.Type == ItemType.RIGHT)
            {
                ScoreManager.Instance.Score = 1;
                //rightItemSound.Play();
            }
            else
            {
                GameManager.Instance.m_OnGameOver?.Invoke();
                //wrongItemSound.Play();
            }
        }
    }
}
