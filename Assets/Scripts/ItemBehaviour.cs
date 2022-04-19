using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ItemBehaviour : MonoBehaviour
{
    [SerializeField] ItemType type;

    public ItemType Type
    {
        get { return type; }
    }

    public UnityEvent m_OnCollision;

    private void Awake()
    {
        if (m_OnCollision == null)
            m_OnCollision = new UnityEvent();
    }
}

public enum ItemType
{
    RIGHT,
    WRONG
}
