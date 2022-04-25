using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ItemBehaviour : MonoBehaviour
{
    [SerializeField] ItemType type;
    public SpawnPoint Point
    {
        get; set;
    }

    public ItemType Type
    {
        get { return type; }
    }

    public UnityEvent m_OnCollision;
    private LTDescr itemTransalte;

    private void OnDisable()
    {
        if (itemTransalte != null)
        {
            LeanTween.cancel(itemTransalte.id);
        }
    }

    private void Awake()
    {
        if (m_OnCollision == null)
            m_OnCollision = new UnityEvent();

        GameManager.Instance.m_OnGameOver.AddListener(OnGameOver);
    }

    public void MoveToY(float yPos, float time)
    {
        itemTransalte = LeanTween.moveY(gameObject, yPos, time);
    }

    private void OnGameOver()
    {
        if (gameObject.activeSelf)
            m_OnCollision?.Invoke();
    }
}

public enum ItemType
{
    None,
    RIGHT,
    WRONG
}
