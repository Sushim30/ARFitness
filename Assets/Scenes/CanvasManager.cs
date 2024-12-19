using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] private GameObject logoWindow;
    [SerializeField] private List<Button> buttons;
    [SerializeField] private List<string> sceneNames;

    public int TodayDay { get; private set; }
    public int TodayMonth { get; private set; }
    private WorkoutTimeDisplay workoutTimeDisplay;

    void Start()
    {
        //workoutTimeDisplay = FindObjectOfType<WorkoutTimeDisplay>();

        //TodayDay = PlayerPrefs.GetInt("TodayDay", DateTime.Now.Day);
        //TodayMonth = PlayerPrefs.GetInt("TodayMonth", DateTime.Now.Month);

        logoWindow.SetActive(true);
        StartCoroutine(LogoInactive());

        for (int i = 0; i < buttons.Count; i++)
        {
            int index = i;
            buttons[i].onClick.AddListener(() => LoadSceneByName(sceneNames[index]));
        }
    }

    private IEnumerator LogoInactive()
    {
        yield return new WaitForSeconds(1f);
        logoWindow.SetActive(false);
    }

    private void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
