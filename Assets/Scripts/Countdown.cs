using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Countdown : MonoBehaviour
{
    [SerializeField] List<GameObject> elements;
    [SerializeField] GameObject tutorialPanel;

    private void OnEnable()
    {
        StartCoroutine(AnimateCountdown());
    }

    IEnumerator AnimateCountdown()
    {
        foreach (GameObject item in elements)
        {
            item.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.6f);
            item.gameObject.SetActive(false);
        }

        gameObject.SetActive(false);
        tutorialPanel.SetActive(true);
    }
}
