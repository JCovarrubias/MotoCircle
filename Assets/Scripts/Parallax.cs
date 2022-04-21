using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] List<GameObject> elements;
    [SerializeField] Transform startPosition;
    [SerializeField] Transform endPosition;
    [SerializeField] float transitionTime;

    private GameManager gameManager;
    private Dictionary<GameObject, LTDescr> elementsTransalte;
    private float totalDistance;

    void Start()
    {
        gameManager = GameManager.Instance;
        gameManager.m_OnStart.AddListener(OnStartTransitions);
        gameManager.m_OnGameOver.AddListener(OnGameOver);

        elementsTransalte = new Dictionary<GameObject, LTDescr>();
        totalDistance = startPosition.position.x + (endPosition.position.x * -1);
    }

    private void OnStartTransitions()
    {
        foreach (GameObject element in elements)
        {
            SetTransition(element);
        }
    }

    private void ResetElement(GameObject element)
    {
        if (gameManager.IsActive)
        {
            element.transform.position = startPosition.position;
            SetTransition(element);
        }
    }

    private void SetTransition(GameObject element)
    {
        float distanceToEnd = element.transform.position.x + (endPosition.position.x * -1);
        float transitionPercentage = 1 - ((totalDistance - distanceToEnd) / totalDistance);

        LTDescr translate = LeanTween.moveX(element, endPosition.position.x, transitionTime * transitionPercentage).setOnComplete(() => ResetElement(element));

        if (!elementsTransalte.ContainsKey(element))
            elementsTransalte.Add(element, translate);
        else
            elementsTransalte[element] = translate;
    }

    private void OnGameOver()
    {
        foreach (KeyValuePair<GameObject, LTDescr> entry in elementsTransalte)
        {
            LeanTween.cancel(entry.Value.id);
        }
    }
}
