using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementDescription : MonoBehaviour
{
    public GameObject descriptionObj;

    private void Start()
    {
        descriptionObj.SetActive(false);
    }
    public void TurnOn()
    {
        descriptionObj.SetActive(true);
        StartCoroutine(Turnoff());
    }

    private IEnumerator Turnoff()
    {
        yield return new WaitForSeconds(3f);
        descriptionObj.SetActive(false);
    }
}
