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
    private LTDescr itemTransalte;

    private void OnDisable()
    {
        if (itemTransalte != null)
            LeanTween.cancel(itemTransalte.id);
    }

    private void Awake()
    {
        if (m_OnCollision == null)
            m_OnCollision = new UnityEvent();
    }

    public void MoveToY(float yPos, float time)
    {
        itemTransalte = LeanTween.moveY(gameObject, yPos, time);
    }
}

public enum ItemType
{
    RIGHT,
    WRONG
}
