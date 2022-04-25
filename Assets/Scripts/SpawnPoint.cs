using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpawnPoint : MonoBehaviour
{
    public UnityEvent m_Emptyoint;
    public SpawnPointType Type
    {
        get; set;
    }

    public ItemBehaviour Item
    {
        get; set;
    }

    public int Index
    {
        get; set;
    }

    private void Awake()
    {
        if (m_Emptyoint == null)
            m_Emptyoint = new UnityEvent();
    }
}

public enum SpawnPointType
{
    INSIDE,
    OUTSIDE,
}
